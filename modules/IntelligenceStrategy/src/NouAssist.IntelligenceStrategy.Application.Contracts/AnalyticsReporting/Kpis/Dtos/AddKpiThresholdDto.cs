using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos
{
    /// <summary>
    /// DTO for adding a threshold to a KPI
    /// </summary>
    public class AddKpiThresholdDto
    {
        /// <summary>
        /// The threshold value
        /// </summary>
        [Required]
        public decimal Value { get; set; }
        
        /// <summary>
        /// Operator for comparison (e.g., >, <, >=, etc.)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Operator { get; set; }
        
        /// <summary>
        /// Status to set when threshold is met
        /// </summary>
        [Required]
        public KpiStatus Status { get; set; }
        
        /// <summary>
        /// Alert condition expression
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxAlertConditionLogicLength)]
        public string AlertExpression { get; set; }
        
        /// <summary>
        /// Alert condition description
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxDescriptionLength)]
        public string AlertDescription { get; set; }
        
        /// <summary>
        /// Whether consecutive occurrences are required to trigger
        /// </summary>
        public bool RequireConsecutiveOccurrences { get; set; }
        
        /// <summary>
        /// Number of consecutive occurrences required
        /// </summary>
        public int ConsecutiveOccurrencesRequired { get; set; } = 1;
        
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
