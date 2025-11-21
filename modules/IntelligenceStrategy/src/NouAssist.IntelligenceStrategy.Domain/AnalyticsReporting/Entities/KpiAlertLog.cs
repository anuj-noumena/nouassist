using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities
{
    /// <summary>
    /// Log of KPI threshold alerts
    /// </summary>
    public class KpiAlertLog : Entity<Guid>
    {
        /// <summary>
        /// The KPI definition this alert is for
        /// </summary>
        public Guid KpiDefinitionId { get; private set; }
        
        /// <summary>
        /// The threshold that triggered the alert
        /// </summary>
        public Guid KpiThresholdId { get; private set; }
        
        /// <summary>
        /// When the alert occurred
        /// </summary>
        public DateTime Timestamp { get; private set; }
        
        /// <summary>
        /// The KPI value that triggered the alert
        /// </summary>
        public decimal Value { get; private set; }
        
        /// <summary>
        /// The KPI status
        /// </summary>
        public KpiStatus Status { get; private set; }
        
        /// <summary>
        /// Whether notification was sent
        /// </summary>
        public bool NotificationSent { get; private set; }
        
        /// <summary>
        /// When notification was sent
        /// </summary>
        public DateTime? NotificationSentAt { get; private set; }
        
        /// <summary>
        /// Alert message
        /// </summary>
        public string Message { get; private set; }
        
        protected KpiAlertLog()
        {
        }
        
        internal KpiAlertLog(
            Guid id,
            Guid kpiDefinitionId,
            Guid kpiThresholdId,
            DateTime timestamp,
            decimal value,
            KpiStatus status,
            string message = null)
            : base(id)
        {
            KpiDefinitionId = kpiDefinitionId;
            KpiThresholdId = kpiThresholdId;
            Timestamp = timestamp;
            Value = value;
            Status = status;
            NotificationSent = false;
            Message = message;
        }
        
        internal void MarkNotificationSent(DateTime sentAt)
        {
            NotificationSent = true;
            NotificationSentAt = sentAt;
        }
    }
}
