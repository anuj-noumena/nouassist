using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for report configuration view model
    /// </summary>
    public class ReportConfigurationDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Name of the report
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the report
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Category of the report
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Data sources used by this report
        /// </summary>
        public ReportDataSourceDto[] DataSources { get; set; }
        
        /// <summary>
        /// Default format for the report
        /// </summary>
        public ReportFormat DefaultFormat { get; set; }
        
        /// <summary>
        /// Export settings
        /// </summary>
        public ExportSettingsDto ExportSettings { get; set; }
        
        /// <summary>
        /// Tags for organizing reports
        /// </summary>
        public string[] Tags { get; set; }
        
        /// <summary>
        /// Report parameters
        /// </summary>
        public ReportParameterDto[] Parameters { get; set; }
        
        /// <summary>
        /// Report schedules
        /// </summary>
        public ReportScheduleDto[] Schedules { get; set; }
    }
}
