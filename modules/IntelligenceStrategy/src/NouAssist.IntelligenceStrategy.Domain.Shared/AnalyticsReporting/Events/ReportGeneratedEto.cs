using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    /// <summary>
    /// Event raised when a report is generated
    /// </summary>
    [EventName("NouAssist.Analytics.ReportGenerated")]
    public class ReportGeneratedEto : AnalyticsEtoBase
    {
        public string ReportName { get; set; }
        public Guid ReportConfigurationId { get; set; }
        public ReportFormat Format { get; set; }
        public DateTime GeneratedAt { get; set; }
        public Guid? GeneratedBy { get; set; }
        public bool IsScheduled { get; set; }
        public string StoragePath { get; set; }
    }
}
