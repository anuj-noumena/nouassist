using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsPayable.Aggregates
{
    public class Vendor : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string VendorName { get; private set; }
        public TaxId TaxId { get; private set; }
        public string ContactEmail { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }
        public int PaymentTermsDays { get; private set; }
        public string PreferredPaymentMethod { get; private set; }
        public MonetaryValue CreditLimit { get; private set; }
        public bool IsActive { get; private set; }

        protected Vendor() { }

        public Vendor(
            Guid id,
            Guid? tenantId,
            string name,
            TaxId taxId,
            string email,
            int paymentTermsDays = 30,
            string preferredPaymentMethod = "ACH") : base(id)
        {
            TenantId = tenantId;
            VendorName = Check.NotNullOrWhiteSpace(name, nameof(name));
            TaxId = taxId;
            ContactEmail = Check.NotNullOrWhiteSpace(email, nameof(email));
            PaymentTermsDays = paymentTermsDays;
            PreferredPaymentMethod = preferredPaymentMethod;
            IsActive = true;
            CreditLimit = MonetaryValue.Zero("USD");
        }

        public void UpdateContactInfo(string email, string phone, string address)
        {
            ContactEmail = Check.NotNullOrWhiteSpace(email, nameof(email));
            Phone = phone;
            Address = address;
        }

        public void SetPaymentTerms(int days, string method)
        {
            if (days < 0)
            {
                throw new BusinessException("AP:InvalidPaymentTerms", 
                    "Payment terms cannot be negative");
            }

            PaymentTermsDays = days;
            PreferredPaymentMethod = method;
        }

        public void SetCreditLimit(MonetaryValue limit)
        {
            if (limit.IsNegative)
            {
                throw new BusinessException("AP:InvalidCreditLimit", 
                    "Credit limit cannot be negative");
            }

            CreditLimit = limit;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
}
