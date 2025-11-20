using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for report generation log view model
    /// </summary>
    public class ReportGenerationLogDto : EntityDto<Guid>
    {
        /// <summary>
        /// The report configuration that was generated
        /// </summary>
        public Guid ReportConfigurationId { get; set; }
        
        /// <summary>
        /// The report schedule that triggered generation (if applicable)
        /// </summary>
        public Guid? ReportScheduleId { get; set; }
        
        /// <summary>
        /// When generation started
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// When generation completed
        /// </summary>
        public DateTime? EndTime { get; set; }
        
        /// <summary>
        /// Status of generation (Success, Failed, etc.)
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Error message if failed
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// Generated file path or URL
        /// </summary>
        public string ResultPath { get; set; }
        
        /// <summary>
        /// Format of the generated report
        /// </summary>
        public ReportFormat Format { get; set; }
        
        /// <summary>
        /// User who triggered the generation
        /// </summary>
        public Guid? UserId { get; set; }
        
        /// <summary>
        /// Parameters used for this generation
        /// </summary>
        public string ParametersJson { get; set; }
    }
}
