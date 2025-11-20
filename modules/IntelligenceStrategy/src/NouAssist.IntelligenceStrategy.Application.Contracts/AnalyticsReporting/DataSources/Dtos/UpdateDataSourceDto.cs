using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos
{
    /// <summary>
    /// DTO for data source update
    /// </summary>
    public class UpdateDataSourceDto
    {
        /// <summary>
        /// Name of the data source
        /// </summary>
        [Required]
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the data source
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Configuration settings as JSON
        /// </summary>
        public string Configuration { get; set; }
        
        /// <summary>
        /// Connection string details
        /// </summary>
        public UpdateConnectionStringDto ConnectionString { get; set; }
        
        /// <summary>
        /// Schema definition as JSON
        /// </summary>
        public string SchemaDefinition { get; set; }
        
        /// <summary>
        /// Refresh interval value (in seconds, minutes, hours, days)
        /// </summary>
        public int RefreshIntervalValue { get; set; }
        
        /// <summary>
        /// Refresh interval unit (seconds, minutes, hours, days)
        /// </summary>
        public string RefreshIntervalUnit { get; set; }
        
        /// <summary>
        /// Whether automatic refresh is enabled
        /// </summary>
        public bool AutoRefreshEnabled { get; set; }
        
        /// <summary>
        /// Whether this data source is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Tags for organizing data sources
        /// </summary>
        public string[] Tags { get; set; }
    }
}
