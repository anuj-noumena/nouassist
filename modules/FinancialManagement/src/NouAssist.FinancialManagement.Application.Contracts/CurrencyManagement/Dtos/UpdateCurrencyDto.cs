using System.ComponentModel.DataAnnotations;
using NouAssist.CurrencyManagement.Enums;

namespace NouAssist.CurrencyManagement.Dtos
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
