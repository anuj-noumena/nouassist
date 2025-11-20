using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for updating a report schedule
    /// </summary>
    public class UpdateReportScheduleDto
    {
        /// <summary>
        /// Cron expression for the schedule
        /// </summary>
        [StringLength(100)]
        public string CronExpression { get; set; }
        
        /// <summary>
        /// Timezone for the schedule
        /// </summary>
        [StringLength(50)]
        public string TimeZone { get; set; }
        
        /// <summary>
        /// Start date for the schedule
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// End date for the schedule
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Whether the schedule is active
        /// </summary>
        public bool? IsActive { get; set; }
        
        /// <summary>
        /// Format of the generated report
        /// </summary>
        public ReportFormat? Format { get; set; }
        
        /// <summary>
        /// Email addresses to send the report to
        /// </summary>
        public string[] Recipients { get; set; }
        
        /// <summary>
        /// Whether to attach the report to the email
        /// </summary>
        public bool? AttachToEmail { get; set; }
        
        /// <summary>
        /// Subject line for the email
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string EmailSubject { get; set; }
        
        /// <summary>
        /// Body text for the email
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxDescriptionLength)]
        public string EmailBody { get; set; }
    }
}
