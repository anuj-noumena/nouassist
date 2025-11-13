using System;
using NouAssist.FinancialManagement.AccountsPayable.ValueObjects;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsPayable.Aggregates
{
    public class Vendor : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string Name { get; private set; }
        public string TaxId { get; private set; }
        public ContactInfo ContactInfo { get; private set; }
        public PaymentTerms DefaultTerms { get; private set; }
        public PaymentMethod PreferredMethod { get; private set; }
        public bool IsActive { get; private set; }
        public MonetaryValue CreditLimit { get; private set; }
        public MonetaryValue CurrentBalance { get; private set; }

        protected Vendor() { } // For ORM

        public Vendor(
            Guid id,
            Guid? tenantId,
            string name,
            ContactInfo contactInfo,
            PaymentTerms defaultTerms,
            PaymentMethod preferredMethod,
            MonetaryValue creditLimit = null,
            string taxId = null)
            : base(id)
        {
            TenantId = tenantId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 200);
            TaxId = taxId;
            ContactInfo = Check.NotNull(contactInfo, nameof(contactInfo));
            DefaultTerms = Check.NotNull(defaultTerms, nameof(defaultTerms));
            PreferredMethod = preferredMethod;
            IsActive = true;
            CreditLimit = creditLimit ?? new MonetaryValue(0, "USD");
            CurrentBalance = new MonetaryValue(0, "USD");
        }

        public void UpdateContactInfo(ContactInfo newContactInfo)
        {
            ContactInfo = Check.NotNull(newContactInfo, nameof(newContactInfo));
        }

        public void UpdatePaymentTerms(PaymentTerms newTerms, PaymentMethod? newMethod = null)
        {
            DefaultTerms = Check.NotNull(newTerms, nameof(newTerms));
            if (newMethod.HasValue)
                PreferredMethod = newMethod.Value;
        }

        public void UpdateCreditLimit(MonetaryValue newLimit)
        {
            if (newLimit.IsNegative)
                throw new BusinessException("AccountsPayable:NegativeCreditLimit",
                    "Credit limit cannot be negative");

            CreditLimit = newLimit;
        }

        public void AddToBalance(MonetaryValue amount)
        {
            CurrentBalance = CurrentBalance.Add(amount);
        }

        public void SubtractFromBalance(MonetaryValue amount)
        {
            CurrentBalance = CurrentBalance.Subtract(amount);
        }

        public void Deactivate()
        {
            if (CurrentBalance.Amount > 0)
                throw new BusinessException("AccountsPayable:VendorHasBalance",
                    "Cannot deactivate vendor with outstanding balance");

            IsActive = false;
        }

        public void Reactivate()
        {
            IsActive = true;
        }
    }
}
