using System;
using NouAssist.Analytics.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.Analytics.DTOs.KPI
{
    /// <summary>
    /// DTO for KPI threshold information
    /// </summary>
    public class KpiThresholdDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public decimal ThresholdValue { get; set; }
        
        public bool IsUpperBound { get; set; }
        
        public KpiStatus StatusWhenBreached { get; set; }
        
        public int Severity { get; set; }
        
        public AlertConditionLogicDto AlertCondition { get; set; }
        
        public string NotificationMessage { get; set; }
    }
}
