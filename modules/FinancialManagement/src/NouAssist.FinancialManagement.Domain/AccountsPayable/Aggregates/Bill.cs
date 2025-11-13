using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.AccountsPayable.Entities;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using NouAssist.FinancialManagement.AccountsPayable.Events;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsPayable.Aggregates
{
    public class Bill : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string BillNumber { get; private set; }
        public Guid VendorId { get; private set; }
        public MonetaryValue Amount { get; private set; }
        public DateTime IssueDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public BillStatus Status { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid? ProjectId { get; private set; }
        public Guid? AttachedDocumentId { get; private set; }
        public bool RequiresApproval { get; private set; }
        public string Notes { get; private set; }

        private readonly List<BillApproval> _approvals = new();
        public IReadOnlyCollection<BillApproval> Approvals => _approvals.AsReadOnly();

        private readonly List<BillLineItem> _lineItems = new();
        public IReadOnlyCollection<BillLineItem> LineItems => _lineItems.AsReadOnly();

        protected Bill() { } // For ORM

        public Bill(
            Guid id,
            Guid? tenantId,
            string billNumber,
            Guid vendorId,
            MonetaryValue amount,
            DateTime issueDate,
            DateTime dueDate,
            Guid categoryId,
            Guid? projectId = null,
            Guid? attachedDocumentId = null,
            string notes = null)
            : base(id)
        {
            if (dueDate < issueDate)
                throw new BusinessException("AccountsPayable:InvalidDueDate",
                    "Due date cannot be before issue date");

            TenantId = tenantId;
            BillNumber = Check.NotNullOrWhiteSpace(billNumber, nameof(billNumber), 50);
            VendorId = vendorId;
            Amount = Check.NotNull(amount, nameof(amount));
            IssueDate = issueDate;
            DueDate = dueDate;
            Status = BillStatus.Draft;
            CategoryId = categoryId;
            ProjectId = projectId;
            AttachedDocumentId = attachedDocumentId;
            Notes = notes;
            RequiresApproval = DetermineIfApprovalRequired(amount);
        }

        public void AddLineItem(BillLineItem lineItem)
        {
            if (Status != BillStatus.Draft)
                throw new BusinessException("AccountsPayable:CannotModifyApprovedBill",
                    "Cannot add line items to non-draft bills");

            _lineItems.Add(lineItem);
            RecalculateAmount();
        }

        public void Submit()
        {
            if (Status != BillStatus.Draft)
                throw new BusinessException("AccountsPayable:BillAlreadySubmitted",
                    "Bill has already been submitted");

            Status = RequiresApproval ? BillStatus.PendingApproval : BillStatus.Approved;

            AddDistributedEvent(new BillCreatedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                BillId = Id,
                BillNumber = BillNumber,
                VendorId = VendorId,
                VendorName = "Vendor", // Should be loaded from Vendor aggregate
                Amount = Amount,
                DueDate = DueDate,
                CategoryId = CategoryId,
                ProjectId = ProjectId,
                RequiresApproval = RequiresApproval,
                CreatedBy = CreatorId.Value,
                TenantId = TenantId
            });
        }

        public void Approve(Guid approvedBy, ApprovalLevel level, string comments = null)
        {
            if (Status != BillStatus.PendingApproval)
                throw new BusinessException("AccountsPayable:BillNotPendingApproval",
                    "Bill is not pending approval");

            var approval = new BillApproval(Id, approvedBy, level, true, comments);
            _approvals.Add(approval);

            // Check if all required approvals are obtained
            if (HasAllRequiredApprovals())
            {
                Status = BillStatus.Approved;
            }
        }

        public void Reject(Guid rejectedBy, ApprovalLevel level, string reason)
        {
            if (Status != BillStatus.PendingApproval)
                throw new BusinessException("AccountsPayable:BillNotPendingApproval",
                    "Bill is not pending approval");

            var approval = new BillApproval(Id, rejectedBy, level, false, reason);
            _approvals.Add(approval);

            Status = BillStatus.Cancelled;
        }

        public void MarkAsPaid(Guid paymentId, DateTime paymentDate, string confirmationNumber)
        {
            if (Status != BillStatus.Approved)
                throw new BusinessException("AccountsPayable:BillNotApproved",
                    "Only approved bills can be marked as paid");

            Status = BillStatus.Paid;

            AddDistributedEvent(new BillPaidEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                BillId = Id,
                PaymentId = paymentId,
                VendorId = VendorId,
                Amount = Amount,
                PaymentDate = paymentDate,
                PaymentMethod = "ACH", // Should come from payment details
                ConfirmationNumber = confirmationNumber,
                CategoryId = CategoryId,
                ProjectId = ProjectId,
                TenantId = TenantId
            });
        }

        private void RecalculateAmount()
        {
            if (_lineItems.Any())
            {
                var total = _lineItems.Sum(li => li.Total.Amount);
                var currency = _lineItems.First().Total.Currency;
                Amount = new MonetaryValue(total, currency);
            }
        }

        private bool DetermineIfApprovalRequired(MonetaryValue amount)
        {
            // Simple threshold-based approval requirement
            return amount.Amount > 1000m;
        }

        private bool HasAllRequiredApprovals()
        {
            // Simplified - should check against approval policy
            return _approvals.Any(a => a.IsApproved);
        }
    }
}
