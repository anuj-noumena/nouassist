using System;
using NouAssist.FinancialManagement.CurrencyManagement.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.CurrencyManagement.Events
{
    [EventName("NouAssist.CurrencyManagement.ExchangeRateUpdated")]
    public class ExchangeRateUpdatedEto : IMultiTenant
    {
        public Guid ExchangeRateId { get; set; }
        public Guid? TenantId { get; set; }
        public string BaseCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal Rate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public ExchangeRateProvider Provider { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
