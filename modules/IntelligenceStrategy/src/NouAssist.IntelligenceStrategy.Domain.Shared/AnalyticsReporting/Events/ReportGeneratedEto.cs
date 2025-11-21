using System;
using NouAssist.Analytics.Enums;
using NouAssist.Analytics.Events.Base;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.EventBus;

namespace NouAssist.Analytics.Events
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
