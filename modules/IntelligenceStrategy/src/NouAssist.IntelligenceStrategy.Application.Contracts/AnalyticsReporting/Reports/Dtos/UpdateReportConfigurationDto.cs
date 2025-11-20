using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for report update
    /// </summary>
    public class UpdateReportConfigurationDto
    {
        /// <summary>
        /// Name of the report
        /// </summary>
        [Required]
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the report
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Category of the report
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string Category { get; set; }
        
        /// <summary>
        /// Data sources used by this report
        /// </summary>
        [Required]
        public Guid[] DataSourceIds { get; set; }
        
        /// <summary>
        /// SQL or query definition for the report
        /// </summary>
        [Required]
        public string QueryDefinition { get; set; }
        
        /// <summary>
        /// Default format for the report
        /// </summary>
        public ReportFormat DefaultFormat { get; set; }
        
        /// <summary>
        /// Report template as JSON or HTML
        /// </summary>
        public string TemplateDefinition { get; set; }
        
        /// <summary>
        /// Export settings
        /// </summary>
        public UpdateExportSettingsDto ExportSettings { get; set; }
        
        /// <summary>
        /// Tags for organizing reports
        /// </summary>
        public string[] Tags { get; set; }
    }
}
