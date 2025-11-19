using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.FinancialManagement.CurrencyManagement.Dtos
{
    public class ConvertCurrencyDto
    {
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3)]
        public string FromCurrency { get; set; }

        [Required]
        [StringLength(3)]
        public string ToCurrency { get; set; }

        public DateTime? AsOfDate { get; set; }
    }

    public class CurrencyConversionResultDto
    {
        public decimal OriginalAmount { get; set; }
        public string FromCurrency { get; set; }
        public decimal ConvertedAmount { get; set; }
        public string ToCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
