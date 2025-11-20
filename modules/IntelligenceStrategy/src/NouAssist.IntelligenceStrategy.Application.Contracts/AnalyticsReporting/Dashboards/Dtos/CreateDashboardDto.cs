using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for dashboard creation
    /// </summary>
    public class CreateDashboardDto
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
        /// ID of the template to create the dashboard from, if any
        /// </summary>
        public Guid? TemplateId { get; set; }
        
        /// <summary>
        /// Tags for organizing dashboards
        /// </summary>
        public string[] Tags { get; set; }
        
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
