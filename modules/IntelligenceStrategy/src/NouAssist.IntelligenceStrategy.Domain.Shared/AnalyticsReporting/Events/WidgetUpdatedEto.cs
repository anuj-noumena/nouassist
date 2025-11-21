using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    [EventName("AnalyticsReporting.Widget.Updated")]
    public class WidgetUpdatedEto : AnalyticsEtoBase
    {
        public Guid Id { get; set; }
        public Guid DashboardId { get; set; }
    }
}
