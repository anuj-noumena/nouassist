using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools.Dtos
{
    /// <summary>
    /// DTO for creating a new OptionPool
    /// </summary>
    public class CreateOptionPoolDto
    {
        [Required]
        public Guid CapTableId { get; set; }
        
        [Required]
        [StringLength(CapTableConsts.MaxNameLength)]
        public string Name { get; set; }
        
        [StringLength(CapTableConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        [Required]
        [Range(1, (double)CapTableConsts.MaxShareQuantity)]
        public decimal TotalReserved { get; set; }
        
        [Required]
        [Range(0.01, 100)]
        public decimal PoolPercentage { get; set; }
        
        // Default vesting terms
        [Required]
        [Range(1, int.MaxValue)]
        public int DefaultVestingPeriodMonths { get; set; } = 48; // 4 years
        
        [Range(0, int.MaxValue)]
        public int DefaultCliffMonths { get; set; } = 12; // 1 year
        
        [Range(0.01, 100)]
        public decimal DefaultVestingPercentage { get; set; } = 100;
    }
}
