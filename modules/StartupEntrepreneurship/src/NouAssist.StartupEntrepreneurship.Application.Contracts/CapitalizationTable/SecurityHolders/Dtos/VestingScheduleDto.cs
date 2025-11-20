using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders.Dtos
{
    /// <summary>
    /// DTO for VestingSchedule information
    /// </summary>
    public class VestingScheduleDto : EntityDto<Guid>
    {
        public Guid SecurityIssuanceId { get; set; }
        public DateTime GrantDate { get; set; }
        public int VestingPeriodMonths { get; set; }
        public int CliffMonths { get; set; }
        public decimal VestingPercentage { get; set; }
        public decimal TotalShares { get; set; }
        public decimal VestedShares { get; set; }
        public string Status { get; set; }
        public DateTime? LastVestingDate { get; set; }
        public DateTime? ExercisedDate { get; set; }
        
        // Security Issuance info
        public string SecurityName { get; set; }
        public string SecurityType { get; set; }
    }
}
