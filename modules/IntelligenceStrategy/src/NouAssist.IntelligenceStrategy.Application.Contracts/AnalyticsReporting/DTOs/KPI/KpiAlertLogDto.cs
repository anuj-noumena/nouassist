using System;
using NouAssist.Analytics.Enums;

namespace NouAssist.Analytics.DTOs.KPI
{
    /// <summary>
    /// DTO for KPI alert log information
    /// </summary>
    public class KpiAlertLogDto
    {
        public Guid Id { get; set; }
        
        public decimal KpiValue { get; set; }
        
        public Guid ThresholdId { get; set; }
        
        public string ThresholdName { get; set; }
        
        public decimal ThresholdValue { get; set; }
        
        public KpiStatus Status { get; set; }
        
        public DateTime OccurredAt { get; set; }
    }
}
