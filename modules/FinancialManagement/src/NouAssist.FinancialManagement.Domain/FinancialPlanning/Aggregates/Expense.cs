using System;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using NouAssist.FinancialManagement.FinancialPlanning.Events;
using NouAssist.FinancialManagement.FinancialPlanning.ValueObjects;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Aggregates
{
    public class Expense : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public Guid UserId { get; private set; }
        public MonetaryValue Amount { get; private set; }
        public DateTime ExpenseDate { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid PayeeId { get; private set; }
        public string Description { get; private set; }
        public ExpenseStatus Status { get; private set; }
        public Guid? ReceiptDocumentId { get; private set; }
        public Guid? ProjectId { get; private set; }
        public bool IsReimbursable { get; private set; }
        public bool IsBillable { get; private set; }
        public MileageData MileageInfo { get; private set; }

        protected Expense() { } // For ORM

        public Expense(
            Guid id,
            Guid? tenantId,
            Guid userId,
            MonetaryValue amount,
            DateTime expenseDate,
            Guid categoryId,
            Guid payeeId,
            string description,
            bool isReimbursable = false,
            bool isBillable = false,
            Guid? projectId = null,
            Guid? receiptDocumentId = null,
            MileageData mileageInfo = null)
            : base(id)
        {
            if (expenseDate > DateTime.UtcNow)
                throw new BusinessException("FinancialPlanning:FutureExpenseDate",
                    "Expense date cannot be in the future");

            TenantId = tenantId;
            UserId = userId;
            Amount = Check.NotNull(amount, nameof(amount));
            ExpenseDate = expenseDate;
            CategoryId = categoryId;
            PayeeId = payeeId;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), 500);
            Status = ExpenseStatus.Draft;
            IsReimbursable = isReimbursable;
            IsBillable = isBillable;
            ProjectId = projectId;
            ReceiptDocumentId = receiptDocumentId;
            MileageInfo = mileageInfo;
        }

        public void Submit()
        {
            if (Status != ExpenseStatus.Draft)
                throw new BusinessException("FinancialPlanning:ExpenseAlreadySubmitted",
                    "Expense has already been submitted");

            ValidateForSubmission();
            Status = ExpenseStatus.Submitted;

            AddDistributedEvent(new ExpenseCreatedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                ExpenseId = Id,
                UserId = UserId,
                Amount = Amount,
                ExpenseDate = ExpenseDate,
                CategoryId = CategoryId,
                PayeeId = PayeeId,
                ProjectId = ProjectId,
                IsReimbursable = IsReimbursable,
                TenantId = TenantId
            });
        }

        public void Approve(Guid approvedBy, string comments = null)
        {
            if (Status != ExpenseStatus.Submitted)
                throw new BusinessException("FinancialPlanning:ExpenseNotSubmitted",
                    "Only submitted expenses can be approved");

            Status = ExpenseStatus.Approved;
        }

        public void Reject(Guid rejectedBy, string reason)
        {
            if (Status != ExpenseStatus.Submitted)
                throw new BusinessException("FinancialPlanning:ExpenseNotSubmitted",
                    "Only submitted expenses can be rejected");

            Status = ExpenseStatus.Rejected;
        }

        public void MarkAsReimbursed()
        {
            if (Status != ExpenseStatus.Approved)
                throw new BusinessException("FinancialPlanning:ExpenseNotApproved",
                    "Only approved expenses can be marked as reimbursed");

            if (!IsReimbursable)
                throw new BusinessException("FinancialPlanning:ExpenseNotReimbursable",
                    "Expense is not marked as reimbursable");

            Status = ExpenseStatus.Reimbursed;
        }

        public void AttachReceipt(Guid documentId)
        {
            ReceiptDocumentId = documentId;
        }

        private void ValidateForSubmission()
        {
            // Policy threshold for receipts (e.g., $25)
            if (Amount.Amount > 25 && ReceiptDocumentId == null)
            {
                throw new BusinessException("FinancialPlanning:ReceiptRequired",
                    "Receipt is required for expenses over $25");
            }

            if (MileageInfo != null && MileageInfo.Miles <= 0)
            {
                throw new BusinessException("FinancialPlanning:InvalidMileage",
                    "Mileage information is invalid");
            }
        }
    }
}
