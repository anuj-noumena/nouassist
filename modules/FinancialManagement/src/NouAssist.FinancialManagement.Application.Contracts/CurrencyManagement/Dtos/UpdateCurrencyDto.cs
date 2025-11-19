using System.ComponentModel.DataAnnotations;
using NouAssist.FinancialManagement.CurrencyManagement.Enums;

namespace NouAssist.FinancialManagement.CurrencyManagement.Dtos
{
    public class UpdateCurrencyDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Symbol { get; set; }

        public CurrencyStatus Status { get; set; }

        public int DisplayOrder { get; set; }
    }
}
