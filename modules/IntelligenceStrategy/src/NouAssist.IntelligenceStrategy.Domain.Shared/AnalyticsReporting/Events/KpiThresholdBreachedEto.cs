using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events.Base;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    /// <summary>
    /// Event raised when a KPI threshold is breached
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.AnalyticsReporting.KpiThresholdBreached")]
    public class KpiThresholdBreachedEto : AnalyticsReportingEtoBase
    {
        /// <summary>
        /// The KPI definition ID
        /// </summary>
        public Guid KpiDefinitionId { get; set; }
        
        /// <summary>
        /// Name of the KPI
        /// </summary>
        public string KpiName { get; set; }
        
        /// <summary>
        /// The current KPI value that triggered the breach
        /// </summary>
        public decimal CurrentValue { get; set; }
        
        /// <summary>
        /// The threshold value that was breached
        /// </summary>
        public decimal ThresholdValue { get; set; }
        
        /// <summary>
        /// The status of the KPI after the breach
        /// </summary>
        public KpiStatus Status { get; set; }
        
        /// <summary>
        /// When the breach occurred
        /// </summary>
        public DateTime BreachTime { get; set; }
    }
}
