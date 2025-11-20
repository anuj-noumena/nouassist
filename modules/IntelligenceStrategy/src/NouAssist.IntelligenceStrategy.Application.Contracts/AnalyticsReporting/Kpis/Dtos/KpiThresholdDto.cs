using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos
{
    /// <summary>
    /// DTO for KPI threshold view model
    /// </summary>
    public class KpiThresholdDto : EntityDto<Guid>
    {
        /// <summary>
        /// The threshold value
        /// </summary>
        public decimal Value { get; set; }
        
        /// <summary>
        /// Operator for comparison (e.g., >, <, >=, etc.)
        /// </summary>
        public string Operator { get; set; }
        
        /// <summary>
        /// Status to set when threshold is met
        /// </summary>
        public KpiStatus Status { get; set; }
        
        /// <summary>
        /// Alert condition logic
        /// </summary>
        public AlertConditionLogicDto AlertCondition { get; set; }
        
        /// <summary>
        /// Whether to send notifications when triggered
        /// </summary>
        public bool SendNotification { get; set; }
        
        /// <summary>
        /// Email recipients for notifications
        /// </summary>
        public string[] NotificationRecipients { get; set; }
    }
}
