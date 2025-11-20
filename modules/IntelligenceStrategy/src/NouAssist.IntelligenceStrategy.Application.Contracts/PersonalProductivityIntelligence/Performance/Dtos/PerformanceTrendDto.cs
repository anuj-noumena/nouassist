using System;
using System.Collections.Generic;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance.Dtos
{
    /// <summary>
    /// DTO for performance trend
    /// </summary>
    public class PerformanceTrendDto
    {
        /// <summary>
        /// Name of the metric
        /// </summary>
        public string MetricName { get; set; }
        
        /// <summary>
        /// Unit of measurement
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// Data points for the trend
        /// </summary>
        public List<PerformanceTrendPointDto> Points { get; set; }
        
        /// <summary>
        /// Overall trend direction (Increasing, Decreasing, Stable)
        /// </summary>
        public string TrendDirection { get; set; }
        
        /// <summary>
        /// Percentage change over the period
        /// </summary>
        public double ChangePercentage { get; set; }
    }
}
