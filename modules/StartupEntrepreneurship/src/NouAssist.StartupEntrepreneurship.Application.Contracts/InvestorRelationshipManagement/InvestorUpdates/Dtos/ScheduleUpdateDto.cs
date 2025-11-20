using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO for scheduling an update
    /// </summary>
    public class ScheduleUpdateDto
    {
        [Required]
        public DateTime ScheduledDate { get; set; }
    }
}
