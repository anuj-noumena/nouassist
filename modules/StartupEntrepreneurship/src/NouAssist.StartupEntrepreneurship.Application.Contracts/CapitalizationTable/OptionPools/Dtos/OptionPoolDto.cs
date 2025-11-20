using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools.Dtos
{
    /// <summary>
    /// DTO for OptionPool view operations
    /// </summary>
    public class OptionPoolDto : FullAuditedEntityDto<Guid>
    {
        public Guid CapTableId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TotalReserved { get; set; }
        public decimal TotalGranted { get; set; }
        public decimal TotalExercised { get; set; }
        public decimal TotalForfeited { get; set; }
        public decimal PoolPercentage { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // Default vesting terms
        public int DefaultVestingPeriodMonths { get; set; }
        public int DefaultCliffMonths { get; set; }
        public decimal DefaultVestingPercentage { get; set; }
        
        // Calculated fields
        public decimal AvailableOptions => TotalReserved - TotalGranted;
        public decimal OutstandingOptions => TotalGranted - TotalExercised - TotalForfeited;
        public decimal UtilizationPercentage => TotalReserved > 0 
            ? Math.Round((TotalGranted / TotalReserved) * 100, 2) 
            : 0;
    }
}
