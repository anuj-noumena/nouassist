using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    /// <summary>
    /// Event raised when a new dashboard is created
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.AnalyticsReporting.DashboardCreated")]
    public class DashboardCreatedEto : AnalyticsReportingEtoBase
    {
        /// <summary>
        /// Name of the dashboard
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// User ID who created the dashboard
        /// </summary>
        public Guid CreatorId { get; set; }
        
        /// <summary>
        /// Whether this dashboard is private to the creator
        /// </summary>
        public bool IsPrivate { get; set; }
    }
}
