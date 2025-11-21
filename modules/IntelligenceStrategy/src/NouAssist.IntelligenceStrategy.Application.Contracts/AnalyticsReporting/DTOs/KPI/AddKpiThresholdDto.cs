using System.ComponentModel.DataAnnotations;
using NouAssist.Analytics.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.Analytics.DTOs.KPI
{
    /// <summary>
    /// DTO for adding a threshold to a KPI
    /// </summary>
    public class AddKpiThresholdDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        public decimal ThresholdValue { get; set; }
        
        public bool IsUpperBound { get; set; }
        
        public KpiStatus StatusWhenBreached { get; set; }
        
        public int Severity { get; set; }
        
        public AlertConditionLogicDto AlertCondition { get; set; }
        
        [StringLength(500)]
        public string NotificationMessage { get; set; }
    }
}
