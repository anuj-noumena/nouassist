using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities
{
    /// <summary>
    /// Schedule for generating a report
    /// </summary>
    public class ReportSchedule : Entity<Guid>
    {
        /// <summary>
        /// The report configuration this schedule belongs to
        /// </summary>
        public Guid ReportConfigurationId { get; private set; }
        
        /// <summary>
        /// When to run the report
        /// </summary>
        public ScheduleTiming Timing { get; private set; }
        
        /// <summary>
        /// Format of the generated report
        /// </summary>
        public ReportFormat Format { get; private set; }
        
        /// <summary>
        /// Email addresses to send the report to
        /// </summary>
        public string[] Recipients { get; private set; }
        
        /// <summary>
        /// Whether to attach the report to the email
        /// </summary>
        public bool AttachToEmail { get; private set; }
        
        /// <summary>
        /// Subject line for the email
        /// </summary>
        public string EmailSubject { get; private set; }
        
        /// <summary>
        /// Body text for the email
        /// </summary>
        public string EmailBody { get; private set; }
        
        /// <summary>
        /// Last time the schedule was executed
        /// </summary>
        public DateTime? LastExecuted { get; private set; }
        
        /// <summary>
        /// Next scheduled execution time
        /// </summary>
        public DateTime? NextScheduled { get; private set; }
        
        protected ReportSchedule()
        {
            Recipients = Array.Empty<string>();
        }
        
        internal ReportSchedule(
            Guid id,
            Guid reportConfigurationId,
            ScheduleTiming timing,
            ReportFormat format,
            string[] recipients = null,
            bool attachToEmail = true,
            string emailSubject = null,
            string emailBody = null)
            : base(id)
        {
            ReportConfigurationId = reportConfigurationId;
            Timing = timing;
            Format = format;
            Recipients = recipients ?? Array.Empty<string>();
            AttachToEmail = attachToEmail;
            EmailSubject = emailSubject;
            EmailBody = emailBody;
        }
        
        internal void UpdateSchedule(ScheduleTiming timing)
        {
            Timing = timing;
        }
        
        internal void SetFormat(ReportFormat format)
        {
            Format = format;
        }
        
        internal void SetRecipients(string[] recipients)
        {
            Recipients = recipients ?? Array.Empty<string>();
        }
        
        internal void SetEmailSettings(bool attachToEmail, string emailSubject, string emailBody)
        {
            AttachToEmail = attachToEmail;
            EmailSubject = emailSubject;
            EmailBody = emailBody;
        }
        
        internal void RecordExecution(DateTime executionTime, DateTime? nextScheduled = null)
        {
            LastExecuted = executionTime;
            NextScheduled = nextScheduled;
        }
    }
}
