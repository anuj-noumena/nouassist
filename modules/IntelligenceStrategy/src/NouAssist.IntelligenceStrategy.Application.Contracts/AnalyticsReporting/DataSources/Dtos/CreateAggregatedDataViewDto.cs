using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos
{
    /// <summary>
    /// DTO for creating an aggregated data view
    /// </summary>
    public class CreateAggregatedDataViewDto
    {
        /// <summary>
        /// Name of the aggregated view
        /// </summary>
        [Required]
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of what this view represents
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Source data sources for this view
        /// </summary>
        [Required]
        public Guid[] SourceDataSourceIds { get; set; }
        
        /// <summary>
        /// Query or aggregation definition as JSON
        /// </summary>
        [Required]
        public string AggregationDefinition { get; set; }
        
        /// <summary>
        /// Refresh interval value (in seconds, minutes, hours, days)
        /// </summary>
        public int RefreshIntervalValue { get; set; } = 5;
        
        /// <summary>
        /// Refresh interval unit (seconds, minutes, hours, days)
        /// </summary>
        public string RefreshIntervalUnit { get; set; } = "minutes";
        
        /// <summary>
        /// Whether automatic refresh is enabled
        /// </summary>
        public bool AutoRefreshEnabled { get; set; } = true;
    }
}
