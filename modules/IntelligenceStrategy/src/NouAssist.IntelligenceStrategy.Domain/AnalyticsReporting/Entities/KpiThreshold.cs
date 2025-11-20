using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities
{
    /// <summary>
    /// Threshold value for a KPI that triggers a status change
    /// </summary>
    public class KpiThreshold : Entity<Guid>
    {
        /// <summary>
        /// The KPI definition this threshold belongs to
        /// </summary>
        public Guid KpiDefinitionId { get; private set; }
        
        /// <summary>
        /// The threshold value
        /// </summary>
        public decimal Value { get; private set; }
        
        /// <summary>
        /// Operator for comparison (e.g., >, <, >=, etc.)
        /// </summary>
        public string Operator { get; private set; }
        
        /// <summary>
        /// Status to set when threshold is met
        /// </summary>
        public KpiStatus Status { get; private set; }
        
        /// <summary>
        /// Alert condition logic
        /// </summary>
        public AlertConditionLogic AlertCondition { get; private set; }
        
        /// <summary>
        /// Whether to send notifications when triggered
        /// </summary>
        public bool SendNotification { get; private set; }
        
        /// <summary>
        /// Email recipients for notifications
        /// </summary>
        public string[] NotificationRecipients { get; private set; }
        
        protected KpiThreshold()
        {
            NotificationRecipients = Array.Empty<string>();
        }
        
        internal KpiThreshold(
            Guid id,
            Guid kpiDefinitionId,
            decimal value,
            string @operator,
            KpiStatus status,
            AlertConditionLogic alertCondition = null,
            bool sendNotification = false,
            string[] notificationRecipients = null)
            : base(id)
        {
            KpiDefinitionId = kpiDefinitionId;
            Value = value;
            Operator = Check.NotNullOrWhiteSpace(@operator, nameof(@operator));
            Status = status;
            AlertCondition = alertCondition;
            SendNotification = sendNotification;
            NotificationRecipients = notificationRecipients ?? Array.Empty<string>();
        }
        
        internal void UpdateThresholdValue(decimal value)
        {
            Value = value;
        }
        
        internal void UpdateOperator(string @operator)
        {
            Operator = Check.NotNullOrWhiteSpace(@operator, nameof(@operator));
        }
        
        internal void UpdateStatus(KpiStatus status)
        {
            Status = status;
        }
        
        internal void SetAlertCondition(AlertConditionLogic alertCondition)
        {
            AlertCondition = alertCondition;
        }
        
        internal void SetNotificationSettings(bool sendNotification, string[] recipients = null)
        {
            SendNotification = sendNotification;
            NotificationRecipients = recipients ?? Array.Empty<string>();
        }
        
        /// <summary>
        /// Check if a value meets this threshold condition
        /// </summary>
        public bool IsThresholdMet(decimal actualValue)
        {
            return Operator switch
            {
                ">" => actualValue > Value,
                ">=" => actualValue >= Value,
                "<" => actualValue < Value,
                "<=" => actualValue <= Value,
                "==" => actualValue == Value,
                "!=" => actualValue != Value,
                _ => false
            };
        }
    }
}
