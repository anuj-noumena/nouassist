using System;
using NouAssist.FinancialManagement.CurrencyManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.CurrencyManagement.Dtos
{
    public class ExchangeRateDto : FullAuditedEntityDto<Guid>
    {
        public string BaseCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal Rate { get; set; }
        public decimal InverseRate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public ExchangeRateProvider Provider { get; set; }
        public string ProviderReference { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool IsActive { get; set; }
    }
}
