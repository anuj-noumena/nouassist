using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.AccountsReceivable.Events;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsReceivable.Aggregates
{
    public class Invoice : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string InvoiceNumber { get; private set; }
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public DateTime IssueDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public InvoiceStatus Status { get; private set; }
        public Guid? ProjectId { get; private set; }
        public MonetaryValue TotalAmount { get; private set; }
        public MonetaryValue PaidAmount { get; private set; }
        public MonetaryValue TaxAmount { get; private set; }

        private readonly List<InvoiceLineItem> _lineItems = new();
        public IReadOnlyCollection<InvoiceLineItem> LineItems => _lineItems.AsReadOnly();

        protected Invoice() { }

        public Invoice(
            Guid id,
            Guid? tenantId,
            string invoiceNumber,
            Guid customerId,
            string customerName,
            DateTime issueDate,
            DateTime dueDate,
            string currency,
            Guid? projectId = null) : base(id)
        {
            TenantId = tenantId;
            InvoiceNumber = Check.NotNullOrWhiteSpace(invoiceNumber, nameof(invoiceNumber));
            CustomerId = customerId;
            CustomerName = Check.NotNullOrWhiteSpace(customerName, nameof(customerName));
            
            if (dueDate < issueDate)
            {
                throw new BusinessException("AR:InvalidDueDate", 
                    "Due date cannot be before issue date");
            }

            IssueDate = issueDate;
            DueDate = dueDate;
            ProjectId = projectId;
            Status = InvoiceStatus.Draft;
            TotalAmount = MonetaryValue.Zero(currency);
            PaidAmount = MonetaryValue.Zero(currency);
            TaxAmount = MonetaryValue.Zero(currency);
        }

        public void AddLineItem(InvoiceLineItem lineItem)
        {
            if (Status != InvoiceStatus.Draft)
            {
                throw new BusinessException("AR:CannotModifyInvoice", 
                    "Cannot add line items to non-draft invoices");
            }

            _lineItems.Add(lineItem);
            RecalculateTotals();
        }

        public void Send()
        {
            if (Status != InvoiceStatus.Draft)
            {
                throw new BusinessException("AR:InvalidStatus", 
                    "Only draft invoices can be sent");
            }

            if (!_lineItems.Any())
            {
                throw new BusinessException("AR:NoLineItems", 
                    "Invoice must have at least one line item");
            }

            Status = InvoiceStatus.Sent;

            AddDistributedEvent(new InvoiceCreatedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                InvoiceId = Id,
                TenantId = TenantId,
                InvoiceNumber = InvoiceNumber,
                CustomerId = CustomerId,
                CustomerName = CustomerName,
                TotalAmount = TotalAmount,
                IssueDate = IssueDate,
                DueDate = DueDate,
                ProjectId = ProjectId,
                CreatedBy = CreatorId.GetValueOrDefault()
            });
        }

        public void ApplyPayment(MonetaryValue paymentAmount, DateTime paymentDate)
        {
            if (Status == InvoiceStatus.Cancelled)
            {
                throw new BusinessException("AR:CannotPayCancelled", 
                    "Cannot apply payment to cancelled invoice");
            }

            var remainingBalance = TotalAmount.Subtract(PaidAmount);
            if (paymentAmount.Amount > remainingBalance.Amount)
            {
                throw new BusinessException("AR:PaymentExceedsBalance", 
                    "Payment amount exceeds invoice balance");
            }

            PaidAmount = PaidAmount.Add(paymentAmount);

            if (PaidAmount.Amount >= TotalAmount.Amount)
            {
                Status = InvoiceStatus.Paid;
            }
            else if (PaidAmount.Amount > 0)
            {
                Status = InvoiceStatus.PartiallyPaid;
            }
        }

        public void Cancel(string reason)
        {
            if (Status == InvoiceStatus.Paid || Status == InvoiceStatus.PartiallyPaid)
            {
                throw new BusinessException("AR:CannotCancelPaid", 
                    "Cannot cancel invoices with payments");
            }

            Status = InvoiceStatus.Cancelled;
        }

        public void PutOnHold(string reason)
        {
            Status = InvoiceStatus.Hold;
        }

        public void RemoveHold()
        {
            if (Status != InvoiceStatus.Hold)
            {
                throw new BusinessException("AR:NotOnHold", 
                    "Invoice is not on hold");
            }

            Status = IsOverdue() ? InvoiceStatus.Overdue : InvoiceStatus.Sent;
        }

        public bool IsOverdue()
        {
            return DateTime.UtcNow.Date > DueDate.Date && 
                   Status != InvoiceStatus.Paid && 
                   Status != InvoiceStatus.Cancelled;
        }

        public MonetaryValue GetRemainingBalance()
        {
            return TotalAmount.Subtract(PaidAmount);
        }

        private void RecalculateTotals()
        {
            var currency = _lineItems.FirstOrDefault()?.SubTotal.Currency ?? "USD";
            var subtotal = _lineItems
                .Aggregate(0m, (sum, item) => sum + item.SubTotal.Amount);

            TotalAmount = new MonetaryValue(subtotal, currency);
        }
    }
}
