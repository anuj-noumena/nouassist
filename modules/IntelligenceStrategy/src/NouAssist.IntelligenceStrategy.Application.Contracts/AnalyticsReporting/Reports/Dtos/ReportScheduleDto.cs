using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for report schedule view model
    /// </summary>
    public class ReportScheduleDto : EntityDto<Guid>
    {
        /// <summary>
        /// When to run the report
        /// </summary>
        public ScheduleTimingDto Timing { get; set; }
        
        /// <summary>
        /// Format of the generated report
        /// </summary>
        public ReportFormat Format { get; set; }
        
        /// <summary>
        /// Email addresses to send the report to
        /// </summary>
        public string[] Recipients { get; set; }
        
        /// <summary>
        /// Whether to attach the report to the email
        /// </summary>
        public bool AttachToEmail { get; set; }
        
        /// <summary>
        /// Subject line for the email
        /// </summary>
        public string EmailSubject { get; set; }
        
        /// <summary>
        /// Body text for the email
        /// </summary>
        public string EmailBody { get; set; }
        
        /// <summary>
        /// Last time the schedule was executed
        /// </summary>
        public DateTime? LastExecuted { get; set; }
        
        /// <summary>
        /// Next scheduled execution time
        /// </summary>
        public DateTime? NextScheduled { get; set; }
    }
}
