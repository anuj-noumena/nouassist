using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos
{
    /// <summary>
    /// DTO for KPI update
    /// </summary>
    public class UpdateKpiDefinitionDto
    {
        /// <summary>
        /// Name of the KPI
        /// </summary>
        [Required]
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of what the KPI measures
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Category of the KPI
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string Category { get; set; }
        
        /// <summary>
        /// Units of measurement for the KPI
        /// </summary>
        [StringLength(50)]
        public string Unit { get; set; }
        
        /// <summary>
        /// Data sources used to calculate the KPI
        /// </summary>
        [Required]
        public Guid[] DataSourceIds { get; set; }
        
        /// <summary>
        /// SQL or query definition to calculate the KPI
        /// </summary>
        [Required]
        public string CalculationDefinition { get; set; }
        
        /// <summary>
        /// Format string for displaying the KPI value
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string DisplayFormat { get; set; }
        
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
        /// Whether this KPI is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Tags for organizing KPIs
        /// </summary>
        public string[] Tags { get; set; }
    }
}
