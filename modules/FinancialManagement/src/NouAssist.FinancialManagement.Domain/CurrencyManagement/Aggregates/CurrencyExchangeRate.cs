using System;
using NouAssist.FinancialManagement.CurrencyManagement.Enums;
using NouAssist.FinancialManagement.CurrencyManagement.Events;
using NouAssist.FinancialManagement.CurrencyManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.CurrencyManagement.Aggregates
{
    public class CurrencyExchangeRate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public CurrencyCode BaseCurrency { get; private set; }
        public CurrencyCode TargetCurrency { get; private set; }
        public ExchangeRate Rate { get; private set; }
        public ExchangeRateProvider Provider { get; private set; }
        public string ProviderReference { get; private set; }
        public DateTime ValidFrom { get; private set; }
        public DateTime? ValidTo { get; private set; }
        public bool IsActive { get; private set; }

        protected CurrencyExchangeRate() { }

        public CurrencyExchangeRate(
            Guid id,
            Guid? tenantId,
            CurrencyCode baseCurrency,
            CurrencyCode targetCurrency,
            decimal rate,
            DateTime effectiveDate,
            ExchangeRateProvider provider,
            string providerReference = null) : base(id)
        {
            if (baseCurrency.Equals(targetCurrency))
            {
                throw new BusinessException("CurrencyManagement:SameCurrencyConversion");
            }

            TenantId = tenantId;
            BaseCurrency = Check.NotNull(baseCurrency, nameof(baseCurrency));
            TargetCurrency = Check.NotNull(targetCurrency, nameof(targetCurrency));
            Rate = new ExchangeRate(rate, effectiveDate);
            Provider = provider;
            ProviderReference = providerReference;
            ValidFrom = effectiveDate.Date;
            ValidTo = null;
            IsActive = true;

            AddDistributedEvent(new ExchangeRateUpdatedEto
            {
                ExchangeRateId = id,
                TenantId = tenantId,
                BaseCurrency = baseCurrency.Code,
                TargetCurrency = targetCurrency.Code,
                Rate = rate,
                EffectiveDate = effectiveDate,
                Provider = provider,
                UpdatedAt = DateTime.UtcNow
            });
        }

        public void UpdateRate(decimal newRate, DateTime effectiveDate)
        {
            Rate = new ExchangeRate(newRate, effectiveDate);
            
            AddDistributedEvent(new ExchangeRateUpdatedEto
            {
                ExchangeRateId = Id,
                TenantId = TenantId,
                BaseCurrency = BaseCurrency.Code,
                TargetCurrency = TargetCurrency.Code,
                Rate = newRate,
                EffectiveDate = effectiveDate,
                Provider = Provider,
                UpdatedAt = DateTime.UtcNow
            });
        }

        public void Deactivate(DateTime validTo)
        {
            IsActive = false;
            ValidTo = validTo.Date;
        }

        public bool IsValidOn(DateTime date)
        {
            return IsActive && 
                   date.Date >= ValidFrom.Date && 
                   (!ValidTo.HasValue || date.Date <= ValidTo.Value.Date);
        }
    }
}
