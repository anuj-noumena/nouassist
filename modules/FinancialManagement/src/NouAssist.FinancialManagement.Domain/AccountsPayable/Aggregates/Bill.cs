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
        public string VendorName { get; private set; }
        public MonetaryValue TotalAmount { get; private set; }
        public DateTime IssueDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public BillStatus Status { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid? ProjectId { get; private set; }
        public Guid? DocumentId { get; private set; }
        public bool RequiresApproval { get; private set; }

        private readonly List<BillApproval> _approvals = new();
        public IReadOnlyCollection<BillApproval> Approvals => _approvals.AsReadOnly();

        private readonly List<BillLineItem> _lineItems = new();
        public IReadOnlyCollection<BillLineItem> LineItems => _lineItems.AsReadOnly();

        protected Bill() { }

        public Bill(
            Guid id,
            Guid? tenantId,
            string billNumber,
            Guid vendorId,
            string vendorName,
            MonetaryValue amount,
            DateTime issueDate,
            DateTime dueDate,
            Guid categoryId,
            Guid? projectId = null,
            Guid? documentId = null,
            bool requiresApproval = true) : base(id)
        {
            TenantId = tenantId;
            BillNumber = Check.NotNullOrWhiteSpace(billNumber, nameof(billNumber));
            VendorId = vendorId;
            VendorName = Check.NotNullOrWhiteSpace(vendorName, nameof(vendorName));
            TotalAmount = Check.NotNull(amount, nameof(amount));
            
            if (dueDate < issueDate)
            {
                throw new BusinessException("AP:InvalidDueDate", 
                    "Due date cannot be before issue date");
            }

            IssueDate = issueDate;
            DueDate = dueDate;
            CategoryId = categoryId;
            ProjectId = projectId;
            DocumentId = documentId;
            Status = BillStatus.Draft;
            RequiresApproval = requiresApproval;

            AddDistributedEvent(new BillCreatedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                BillId = Id,
                TenantId = TenantId,
                BillNumber = BillNumber,
                VendorId = VendorId,
                VendorName = VendorName,
                Amount = TotalAmount,
                DueDate = DueDate,
                CategoryId = CategoryId,
                ProjectId = ProjectId,
                RequiresApproval = RequiresApproval,
                CreatedBy = CreatorId.GetValueOrDefault()
            });
        }

        public void AddLineItem(BillLineItem lineItem)
        {
            if (Status != BillStatus.Draft)
            {
                throw new BusinessException("AP:CannotModifyBill", 
                    "Cannot add line items to non-draft bills");
            }

            _lineItems.Add(lineItem);
        }

        public void Approve(Guid approverId, ApprovalLevel level, string comments = null)
        {
            if (Status != BillStatus.PendingApproval && Status != BillStatus.Draft)
            {
                throw new BusinessException("AP:InvalidStatus", 
                    "Bill cannot be approved in current status");
            }

            var approval = new BillApproval(Id, approverId, level, comments);
            _approvals.Add(approval);

            if (HasAllRequiredApprovals())
            {
                Status = BillStatus.Approved;
            }
            else
            {
                Status = BillStatus.PendingApproval;
            }
        }

        public void MarkAsPaid(Guid paymentId, DateTime paymentDate, string confirmationNumber)
        {
            if (Status != BillStatus.Approved && Status != BillStatus.Scheduled)
            {
                throw new BusinessException("AP:CannotPay", 
                    "Only approved or scheduled bills can be paid");
            }

            Status = BillStatus.Paid;

            AddDistributedEvent(new BillPaidEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                BillId = Id,
                TenantId = TenantId,
                PaymentId = paymentId,
                VendorId = VendorId,
                VendorName = VendorName,
                Amount = TotalAmount,
                PaymentDate = paymentDate,
                PaymentMethod = "ACH", // Simplified
                ConfirmationNumber = confirmationNumber,
                CategoryId = CategoryId,
                ProjectId = ProjectId
            });
        }

        public void Cancel(string reason)
        {
            if (Status == BillStatus.Paid)
            {
                throw new BusinessException("AP:CannotCancelPaid", 
                    "Cannot cancel paid bills");
            }

            Status = BillStatus.Cancelled;
        }

        private bool HasAllRequiredApprovals()
        {
            // Simplified - in production, check against policy
            return _approvals.Count >= 1;
        }

        public bool IsOverdue()
        {
            return DateTime.UtcNow.Date > DueDate.Date && Status != BillStatus.Paid;
        }
    }
}
