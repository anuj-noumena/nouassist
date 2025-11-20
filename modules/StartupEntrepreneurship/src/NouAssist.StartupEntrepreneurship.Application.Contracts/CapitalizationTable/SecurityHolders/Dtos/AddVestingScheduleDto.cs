using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders.Dtos
{
    /// <summary>
    /// DTO for adding a vesting schedule to a security holder
    /// </summary>
    public class AddVestingScheduleDto
    {
        [Required]
        public Guid SecurityIssuanceId { get; set; }
        
        [Required]
        public DateTime GrantDate { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int VestingPeriodMonths { get; set; }
        
        [Range(0, int.MaxValue)]
        public int CliffMonths { get; set; } = 0;
        
        [Range(0.01, 100)]
        public decimal VestingPercentage { get; set; } = 100;
        
        [Required]
        [Range(1, (double)CapTableConsts.MaxShareQuantity)]
        public decimal TotalShares { get; set; }
    }
}
