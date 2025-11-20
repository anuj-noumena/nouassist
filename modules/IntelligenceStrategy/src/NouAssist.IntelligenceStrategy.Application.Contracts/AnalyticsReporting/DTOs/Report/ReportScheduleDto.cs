using System;

namespace NouAssist.Analytics.DTOs.Report
{
    /// <summary>
    /// DTO for report schedule configuration
    /// </summary>
    public class ReportScheduleDto
    {
        public bool IsEnabled { get; set; }
        
        public string CronExpression { get; set; }
        
        public DateTime NextRunTime { get; set; }
        
        public DateTime? LastRunTime { get; set; }
        
        public string EmailRecipients { get; set; }
        
        public bool StoreInRepository { get; set; }
    }
}
