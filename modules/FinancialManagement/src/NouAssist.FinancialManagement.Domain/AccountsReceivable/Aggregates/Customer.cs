using System;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.AccountsReceivable.ValueObjects;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsReceivable.Aggregates
{
    public class Customer : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string Name { get; private set; }
        public string TaxId { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public CustomerType Type { get; private set; }
        public CreditTerms CreditTerms { get; private set; }
        public MonetaryValue CurrentBalance { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsCreditSuspended { get; private set; }

        protected Customer() { } // For ORM

        public Customer(
            Guid id,
            Guid? tenantId,
            string name,
            string email,
            CustomerType type,
            CreditTerms creditTerms,
            string taxId = null,
            string phone = null)
            : base(id)
        {
            TenantId = tenantId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 200);
            Email = Check.NotNullOrWhiteSpace(email, nameof(email), 200);
            TaxId = taxId;
            Phone = phone;
            Type = type;
            CreditTerms = Check.NotNull(creditTerms, nameof(creditTerms));
            CurrentBalance = new MonetaryValue(0, creditTerms.Currency);
            IsActive = true;
            IsCreditSuspended = false;
        }

        public void UpdateCreditTerms(CreditTerms newTerms, string justification)
        {
            if (string.IsNullOrWhiteSpace(justification))
                throw new BusinessException("AccountsReceivable:JustificationRequired",
                    "Justification required for credit terms change");

            CreditTerms = Check.NotNull(newTerms, nameof(newTerms));
        }

        public void AddToBalance(MonetaryValue amount)
        {
            CurrentBalance = CurrentBalance.Add(amount);
        }

        public void SubtractFromBalance(MonetaryValue amount)
        {
            CurrentBalance = CurrentBalance.Subtract(amount);
        }

        public void SuspendCredit(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new BusinessException("AccountsReceivable:ReasonRequired",
                    "Reason required for credit suspension");

            IsCreditSuspended = true;
        }

        public void RestoreCredit()
        {
            IsCreditSuspended = false;
        }

        public void Deactivate()
        {
            if (CurrentBalance.Amount > 0)
                throw new BusinessException("AccountsReceivable:CustomerHasBalance",
                    "Cannot deactivate customer with outstanding balance");

            IsActive = false;
        }

        public void Reactivate()
        {
            IsActive = true;
        }

        public bool CanExtendCredit(MonetaryValue requestedAmount)
        {
            if (!IsActive || IsCreditSuspended)
                return false;

            var proposedBalance = CurrentBalance.Amount + requestedAmount.Amount;
            return proposedBalance <= CreditTerms.CreditLimit;
        }
    }
}
