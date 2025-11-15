using System;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
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

        protected Expense() { }

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
            Guid? receiptDocumentId = null,
            Guid? projectId = null) : base(id)
        {
            TenantId = tenantId;
            UserId = userId;
            Amount = Check.NotNull(amount, nameof(amount));
            
            if (expenseDate > DateTime.UtcNow.AddDays(1))
                throw new BusinessException("FPA:InvalidExpenseDate", 
                    "Expense date cannot be in the future");
            
            ExpenseDate = expenseDate;
            CategoryId = categoryId;
            PayeeId = payeeId;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            Status = ExpenseStatus.Draft;
            IsReimbursable = isReimbursable;
            IsBillable = isBillable;
            ReceiptDocumentId = receiptDocumentId;
            ProjectId = projectId;
        }

        public void Submit()
        {
            if (Status != ExpenseStatus.Draft)
                throw new BusinessException("FPA:ExpenseAlreadySubmitted", 
                    "Expense has already been submitted");

            ValidateForSubmission();
            Status = ExpenseStatus.Submitted;
        }

        public void Approve(Guid approvedBy)
        {
            if (Status != ExpenseStatus.Submitted)
                throw new BusinessException("FPA:InvalidStatus", 
                    "Only submitted expenses can be approved");

            Status = ExpenseStatus.Approved;
        }

        public void Reject(Guid rejectedBy, string reason)
        {
            if (Status != ExpenseStatus.Submitted)
                throw new BusinessException("FPA:InvalidStatus", 
                    "Only submitted expenses can be rejected");

            Status = ExpenseStatus.Rejected;
        }

        public void MarkAsReimbursed()
        {
            if (Status != ExpenseStatus.Approved)
                throw new BusinessException("FPA:NotApproved", 
                    "Only approved expenses can be reimbursed");

            if (!IsReimbursable)
                throw new BusinessException("FPA:NotReimbursable", 
                    "Expense is not marked as reimbursable");

            Status = ExpenseStatus.Reimbursed;
        }

        private void ValidateForSubmission()
        {
            // Receipt required for expenses over $25
            if (Amount.Amount > 25 && ReceiptDocumentId == null)
            {
                throw new BusinessException("FPA:ReceiptRequired", 
                    "Receipt is required for expenses over $25");
            }
        }
    }
}
