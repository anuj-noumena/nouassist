using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Analytics.DTOs.Report
{
    /// <summary>
    /// DTO for updating a report schedule
    /// </summary>
    public class UpdateReportScheduleDto
    {
        public bool IsEnabled { get; set; }
        
        [Required]
        public string CronExpression { get; set; }
        
        public DateTime NextRunTime { get; set; }
        
        public string EmailRecipients { get; set; }
        
        public bool StoreInRepository { get; set; }
    }
}
