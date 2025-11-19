using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.FinancialManagement.CurrencyManagement.Enums;

namespace NouAssist.FinancialManagement.CurrencyManagement.Dtos
{
    public class CreateExchangeRateDto
    {
        [Required]
        [StringLength(3)]
        public string BaseCurrency { get; set; }

        [Required]
        [StringLength(3)]
        public string TargetCurrency { get; set; }

        [Required]
        [Range(0.000001, double.MaxValue)]
        public decimal Rate { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }

        public ExchangeRateProvider Provider { get; set; } = ExchangeRateProvider.Manual;

        [StringLength(256)]
        public string ProviderReference { get; set; }
    }
}
