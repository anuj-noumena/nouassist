using System;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
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
        public TaxId TaxId { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string BillingAddress { get; private set; }
        public CustomerType Type { get; private set; }
        public int CreditTermsDays { get; private set; }
        public MonetaryValue CreditLimit { get; private set; }
        public MonetaryValue CurrentBalance { get; private set; }
        public bool IsActive { get; private set; }
        public bool CreditOnHold { get; private set; }

        protected Customer() { }

        public Customer(
            Guid id,
            Guid? tenantId,
            string name,
            TaxId taxId,
            string email,
            CustomerType type,
            int creditTermsDays = 30) : base(id)
        {
            TenantId = tenantId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            TaxId = taxId;
            Email = Check.NotNullOrWhiteSpace(email, nameof(email));
            Type = type;
            CreditTermsDays = creditTermsDays;
            IsActive = true;
            CreditOnHold = false;
            CreditLimit = MonetaryValue.Zero("USD");
            CurrentBalance = MonetaryValue.Zero("USD");
        }

        public void UpdateCreditLimit(MonetaryValue newLimit, string justification)
        {
            if (newLimit.IsNegative)
                throw new BusinessException("AR:InvalidCreditLimit", 
                    "Credit limit cannot be negative");

            CreditLimit = newLimit;
        }

        public void SuspendCredit(string reason)
        {
            CreditOnHold = true;
        }

        public void RestoreCredit()
        {
            CreditOnHold = false;
        }

        public void UpdateBalance(MonetaryValue amount, bool increase)
        {
            CurrentBalance = increase
                ? CurrentBalance.Add(amount)
                : CurrentBalance.Subtract(amount);
        }

        public bool CanExtendCredit(MonetaryValue requestedAmount)
        {
            if (CreditOnHold)
                return false;

            var proposedBalance = CurrentBalance.Add(requestedAmount);
            return proposedBalance.Amount <= CreditLimit.Amount;
        }

        public void Deactivate()
        {
            if (CurrentBalance.Amount > 0)
                throw new BusinessException("AR:CannotDeactivateWithBalance", 
                    "Cannot deactivate customer with outstanding balance");

            IsActive = false;
        }
    }
}
