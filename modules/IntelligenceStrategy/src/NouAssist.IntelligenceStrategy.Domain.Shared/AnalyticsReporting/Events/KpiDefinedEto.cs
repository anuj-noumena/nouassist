using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    /// <summary>
    /// Event raised when a new KPI is defined
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.AnalyticsReporting.KpiDefined")]
    public class KpiDefinedEto : AnalyticsReportingEtoBase
    {
        /// <summary>
        /// Name of the KPI
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of what the KPI measures
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// User ID who defined the KPI
        /// </summary>
        public Guid CreatorId { get; set; }
        
        /// <summary>
        /// Whether the KPI has alerts defined
        /// </summary>
        public bool HasAlerts { get; set; }
    }
}
