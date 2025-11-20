using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools.Dtos
{
    /// <summary>
    /// DTO for expanding an option pool
    /// </summary>
    public class ExpandPoolDto
    {
        [Required]
        [Range(1, (double)CapTableConsts.MaxShareQuantity)]
        public decimal AdditionalShares { get; set; }
        
        [Required]
        [Range(0.01, 100)]
        public decimal NewPoolPercentage { get; set; }
    }
}
