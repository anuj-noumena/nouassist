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
        public DateTime IssueDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public MonetaryValue TotalAmount { get; private set; }
        public MonetaryValue PaidAmount { get; private set; }
        public MonetaryValue Balance { get; private set; }
        public InvoiceStatus Status { get; private set; }
        public Guid? ProjectId { get; private set; }
        public string Notes { get; private set; }

        private readonly List<InvoiceLineItem> _lineItems = new();
        public IReadOnlyCollection<InvoiceLineItem> LineItems => _lineItems.AsReadOnly();

        private readonly List<InvoicePaymentApplication> _paymentApplications = new();
        public IReadOnlyCollection<InvoicePaymentApplication> PaymentApplications => 
            _paymentApplications.AsReadOnly();

        protected Invoice() { } // For ORM

        public Invoice(
            Guid id,
            Guid? tenantId,
            string invoiceNumber,
            Guid customerId,
            DateTime issueDate,
            DateTime dueDate,
            Guid? projectId = null,
            string notes = null)
            : base(id)
        {
            if (dueDate < issueDate)
                throw new BusinessException("AccountsReceivable:InvalidDueDate",
                    "Due date cannot be before issue date");

            TenantId = tenantId;
            InvoiceNumber = Check.NotNullOrWhiteSpace(invoiceNumber, nameof(invoiceNumber), 50);
            CustomerId = customerId;
            IssueDate = issueDate;
            DueDate = dueDate;
            Status = InvoiceStatus.Draft;
            ProjectId = projectId;
            Notes = notes;
            TotalAmount = new MonetaryValue(0, "USD");
            PaidAmount = new MonetaryValue(0, "USD");
            Balance = new MonetaryValue(0, "USD");
        }

        public void AddLineItem(InvoiceLineItem lineItem)
        {
            if (Status != InvoiceStatus.Draft)
                throw new BusinessException("AccountsReceivable:CannotModifySentInvoice",
                    "Cannot add line items to sent invoices");

            _lineItems.Add(lineItem);
            RecalculateTotals();
        }

        public void Send(string customerName)
        {
            if (Status != InvoiceStatus.Draft)
                throw new BusinessException("AccountsReceivable:InvoiceAlreadySent",
                    "Invoice has already been sent");

            if (!_lineItems.Any())
                throw new BusinessException("AccountsReceivable:NoLineItems",
                    "Invoice must have at least one line item");

            Status = InvoiceStatus.Sent;

            var lineItemDtos = _lineItems.Select(li => new InvoiceLineItemDto
            {
                Description = li.Description,
                Quantity = li.Quantity,
                UnitPrice = li.UnitPrice,
                Total = li.Total
            }).ToList();

            AddDistributedEvent(new InvoiceCreatedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                InvoiceId = Id,
                InvoiceNumber = InvoiceNumber,
                CustomerId = CustomerId,
                CustomerName = customerName,
                TotalAmount = TotalAmount,
                IssueDate = IssueDate,
                DueDate = DueDate,
                ProjectId = ProjectId,
                LineItems = lineItemDtos,
                CreatedBy = CreatorId.Value,
                TenantId = TenantId
            });
        }

        public void ApplyPayment(Guid paymentId, MonetaryValue amount, Guid appliedBy)
        {
            if (Status == InvoiceStatus.Cancelled)
                throw new BusinessException("AccountsReceivable:CannotPayCancelledInvoice",
                    "Cannot apply payment to cancelled invoice");

            if (amount.IsNegative || amount.IsZero)
                throw new BusinessException("AccountsReceivable:InvalidPaymentAmount",
                    "Payment amount must be positive");

            if (amount.Amount > Balance.Amount)
                throw new BusinessException("AccountsReceivable:PaymentExceedsBalance",
                    "Payment amount exceeds invoice balance");

            var application = new InvoicePaymentApplication(
                Id, 
                paymentId, 
                amount, 
                DateTime.UtcNow);

            _paymentApplications.Add(application);

            PaidAmount = PaidAmount.Add(amount);
            Balance = TotalAmount.Subtract(PaidAmount);

            var isFullyPaid = Balance.IsZero;
            Status = isFullyPaid ? InvoiceStatus.Paid : InvoiceStatus.PartiallyPaid;

            AddDistributedEvent(new InvoicePaymentAppliedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                InvoiceId = Id,
                PaymentId = paymentId,
                AppliedAmount = amount,
                RemainingBalance = Balance,
                IsFullyPaid = isFullyPaid,
                AppliedBy = appliedBy,
                TenantId = TenantId
            });
        }

        public void MarkAsOverdue()
        {
            if (Status != InvoiceStatus.Sent && Status != InvoiceStatus.PartiallyPaid)
                return;

            var daysOverdue = (DateTime.Today - DueDate).Days;
            if (daysOverdue <= 0)
                return;

            Status = InvoiceStatus.Overdue;

            AddDistributedEvent(new InvoiceOverdueEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                InvoiceId = Id,
                CustomerId = CustomerId,
                OverdueAmount = Balance,
                DaysOverdue = daysOverdue,
                OriginalDueDate = DueDate,
                TimeZone = TimeZoneInfo.Utc,
                TenantId = TenantId
            });
        }

        public void Cancel(string reason)
        {
            if (Status == InvoiceStatus.Paid)
                throw new BusinessException("AccountsReceivable:CannotCancelPaidInvoice",
                    "Cannot cancel paid invoice. Create a credit memo instead.");

            if (PaidAmount.Amount > 0)
                throw new BusinessException("AccountsReceivable:CannotCancelPartiallyPaid",
                    "Cannot cancel partially paid invoice. Refund payment first.");

            Status = InvoiceStatus.Cancelled;
        }

        public void PlaceOnHold(string reason)
        {
            if (Status == InvoiceStatus.Paid || Status == InvoiceStatus.Cancelled)
                throw new BusinessException("AccountsReceivable:CannotHoldPaidOrCancelled",
                    "Cannot place paid or cancelled invoice on hold");

            Status = InvoiceStatus.Hold;
        }

        public void RemoveHold()
        {
            if (Status != InvoiceStatus.Hold)
                throw new BusinessException("AccountsReceivable:InvoiceNotOnHold",
                    "Invoice is not on hold");

            Status = Balance.IsZero ? InvoiceStatus.Paid : InvoiceStatus.Sent;
        }

        private void RecalculateTotals()
        {
            if (_lineItems.Any())
            {
                var total = _lineItems.Sum(li => li.Total.Amount);
                var currency = _lineItems.First().Total.Currency;
                TotalAmount = new MonetaryValue(total, currency);
                Balance = TotalAmount.Subtract(PaidAmount);
            }
        }
    }
}
