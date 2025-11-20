using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos
{
    /// <summary>
    /// DTO for KPI alert log view model
    /// </summary>
    public class KpiAlertLogDto : EntityDto<Guid>
    {
        /// <summary>
        /// The KPI definition this alert is for
        /// </summary>
        public Guid KpiDefinitionId { get; set; }
        
        /// <summary>
        /// The KPI name for display purposes
        /// </summary>
        public string KpiName { get; set; }
        
        /// <summary>
        /// The threshold that triggered the alert
        /// </summary>
        public Guid KpiThresholdId { get; set; }
        
        /// <summary>
        /// When the alert occurred
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// The KPI value that triggered the alert
        /// </summary>
        public decimal Value { get; set; }
        
        /// <summary>
        /// The KPI value formatted for display
        /// </summary>
        public string FormattedValue { get; set; }
        
        /// <summary>
        /// The KPI status
        /// </summary>
        public KpiStatus Status { get; set; }
        
        /// <summary>
        /// Whether notification was sent
        /// </summary>
        public bool NotificationSent { get; set; }
        
        /// <summary>
        /// When notification was sent
        /// </summary>
        public DateTime? NotificationSentAt { get; set; }
        
        /// <summary>
        /// Alert message
        /// </summary>
        public string Message { get; set; }
    }
}
