using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    [EventName("AnalyticsReporting.DashboardLayout.Updated")]
    public class DashboardLayoutUpdatedEto : AnalyticsEtoBase
    {
        public Guid Id { get; set; }
    }
}
