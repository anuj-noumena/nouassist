using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools.Dtos
{
    /// <summary>
    /// DTO for updating an existing OptionPool
    /// </summary>
    public class UpdateOptionPoolDto
    {
        [Required]
        [StringLength(CapTableConsts.MaxNameLength)]
        public string Name { get; set; }
        
        [StringLength(CapTableConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        // Default vesting terms
        [Range(1, int.MaxValue)]
        public int? DefaultVestingPeriodMonths { get; set; }
        
        [Range(0, int.MaxValue)]
        public int? DefaultCliffMonths { get; set; }
        
        [Range(0.01, 100)]
        public decimal? DefaultVestingPercentage { get; set; }
    }
}
