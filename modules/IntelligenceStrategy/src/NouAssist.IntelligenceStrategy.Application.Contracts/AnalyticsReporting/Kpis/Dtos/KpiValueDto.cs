using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos
{
    /// <summary>
    /// DTO for KPI value
    /// </summary>
    public class KpiValueDto
    {
        /// <summary>
        /// The numeric value of the KPI
        /// </summary>
        public decimal Value { get; set; }
        
        /// <summary>
        /// The timestamp when this value was measured
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// The status of the KPI based on thresholds
        /// </summary>
        public KpiStatus Status { get; set; }
        
        /// <summary>
        /// Optional formatted display value
        /// </summary>
        public string FormattedValue { get; set; }
    }
}
