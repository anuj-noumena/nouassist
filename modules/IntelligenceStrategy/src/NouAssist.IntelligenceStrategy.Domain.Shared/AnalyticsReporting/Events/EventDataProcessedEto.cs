using System;
using NouAssist.Analytics.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.Analytics.Events
{
    /// <summary>
    /// Internal event raised when an external event has been processed and stored
    /// </summary>
    [EventName("NouAssist.Analytics.EventDataProcessed")]
    public class EventDataProcessedEto : AnalyticsEtoBase
    {
        public string SourceEventName { get; set; }
        public Guid DataSourceId { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string AggregatedViewName { get; set; }
        public bool ProcessingSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }
}
