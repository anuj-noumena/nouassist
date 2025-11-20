using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for dashboard update
    /// </summary>
    public class UpdateDashboardDto
    {
        /// <summary>
        /// Name of the dashboard
        /// </summary>
        [Required]
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the dashboard
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Icon for the dashboard
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string Icon { get; set; }
        
        /// <summary>
        /// Whether this dashboard is private to the creator
        /// </summary>
        public bool IsPrivate { get; set; }
        
        /// <summary>
        /// Tags for organizing dashboards
        /// </summary>
        public string[] Tags { get; set; }
        
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
    }
}
