using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    [EventName("AnalyticsReporting.Widget.Removed")]
    public class WidgetRemovedEto : AnalyticsEtoBase
    {
        public Guid Id { get; set; }
        public Guid DashboardId { get; set; }
    }
}
