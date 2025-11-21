using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities
{
    /// <summary>
    /// Log of report generation executions
    /// </summary>
    public class ReportGenerationLog : Entity<Guid>
    {
        /// <summary>
        /// The report configuration that was generated
        /// </summary>
        public Guid ReportConfigurationId { get; private set; }
        
        /// <summary>
        /// The report schedule that triggered generation (if applicable)
        /// </summary>
        public Guid? ReportScheduleId { get; private set; }
        
        /// <summary>
        /// When generation started
        /// </summary>
        public DateTime StartTime { get; private set; }
        
        /// <summary>
        /// When generation completed
        /// </summary>
        public DateTime? EndTime { get; private set; }
        
        /// <summary>
        /// Status of generation (Success, Failed, etc.)
        /// </summary>
        public string Status { get; private set; }
        
        /// <summary>
        /// Error message if failed
        /// </summary>
        public string ErrorMessage { get; private set; }
        
        /// <summary>
        /// Generated file path or URL
        /// </summary>
        public string ResultPath { get; private set; }
        
        /// <summary>
        /// Format of the generated report
        /// </summary>
        public ReportFormat Format { get; private set; }
        
        /// <summary>
        /// User who triggered the generation
        /// </summary>
        public Guid? UserId { get; private set; }
        
        /// <summary>
        /// Parameters used for this generation
        /// </summary>
        public string ParametersJson { get; private set; }
        
        protected ReportGenerationLog()
        {
        }
        
        internal ReportGenerationLog(
            Guid id,
            Guid reportConfigurationId,
            DateTime startTime,
            ReportFormat format,
            Guid? reportScheduleId = null,
            Guid? userId = null,
            string parametersJson = null)
            : base(id)
        {
            ReportConfigurationId = reportConfigurationId;
            ReportScheduleId = reportScheduleId;
            StartTime = startTime;
            Status = "Processing";
            Format = format;
            UserId = userId;
            ParametersJson = parametersJson;
        }
        
        internal void MarkAsCompleted(DateTime endTime, string resultPath)
        {
            EndTime = endTime;
            Status = "Completed";
            ResultPath = resultPath;
        }
        
        internal void MarkAsFailed(DateTime endTime, string errorMessage)
        {
            EndTime = endTime;
            Status = "Failed";
            ErrorMessage = errorMessage;
        }
    }
}
