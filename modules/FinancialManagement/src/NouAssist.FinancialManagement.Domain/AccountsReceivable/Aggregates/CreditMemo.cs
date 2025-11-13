using System;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsReceivable.Aggregates
{
    public class CreditMemo : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string CreditMemoNumber { get; private set; }
        public Guid CustomerId { get; private set; }
        public MonetaryValue Amount { get; private set; }
        public DateTime IssueDate { get; private set; }
        public CreditMemoReason Reason { get; private set; }
        public string Description { get; private set; }
        public Guid? OriginalInvoiceId { get; private set; }
        public bool IsApplied { get; private set; }

        protected CreditMemo() { } // For ORM

        public CreditMemo(
            Guid id,
            Guid? tenantId,
            string creditMemoNumber,
            Guid customerId,
            MonetaryValue amount,
            DateTime issueDate,
            CreditMemoReason reason,
            string description,
            Guid? originalInvoiceId = null)
            : base(id)
        {
            if (amount.IsNegative || amount.IsZero)
                throw new BusinessException("AccountsReceivable:InvalidCreditAmount",
                    "Credit memo amount must be positive");

            TenantId = tenantId;
            CreditMemoNumber = Check.NotNullOrWhiteSpace(creditMemoNumber, nameof(creditMemoNumber), 50);
            CustomerId = customerId;
            Amount = amount;
            IssueDate = issueDate;
            Reason = reason;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), 500);
            OriginalInvoiceId = originalInvoiceId;
            IsApplied = false;
        }

        public void Apply()
        {
            if (IsApplied)
                throw new BusinessException("AccountsReceivable:CreditMemoAlreadyApplied",
                    "Credit memo has already been applied");

            IsApplied = true;
        }
    }
}
