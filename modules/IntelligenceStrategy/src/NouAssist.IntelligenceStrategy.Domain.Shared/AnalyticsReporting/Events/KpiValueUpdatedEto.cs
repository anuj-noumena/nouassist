using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    [EventName("AnalyticsReporting.Kpi.ValueUpdated")]
    public class KpiValueUpdatedEto : AnalyticsEtoBase
    {
        public Guid KpiDefinitionId { get; set; }
        public decimal NewValue { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
