using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    /// <summary>
    /// Event raised when a new report is defined
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.AnalyticsReporting.ReportDefined")]
    public class ReportDefinedEto : AnalyticsReportingEtoBase
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
        /// User ID who defined the report
        /// </summary>
        public Guid CreatorId { get; set; }
        
        /// <summary>
        /// Whether the report is scheduled
        /// </summary>
        public bool IsScheduled { get; set; }
    }
}
