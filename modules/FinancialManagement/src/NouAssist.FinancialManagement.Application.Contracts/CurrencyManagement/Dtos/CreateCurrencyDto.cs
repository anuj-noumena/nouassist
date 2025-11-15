using System.ComponentModel.DataAnnotations;

namespace NouAssist.CurrencyManagement.Dtos
{
    public class CreateCurrencyDto
    {
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Code { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Symbol { get; set; }

        [Range(0, 8)]
        public int DecimalPlaces { get; set; } = 2;

        public bool IsBaseCurrency { get; set; }

        public int DisplayOrder { get; set; }
    }
}
