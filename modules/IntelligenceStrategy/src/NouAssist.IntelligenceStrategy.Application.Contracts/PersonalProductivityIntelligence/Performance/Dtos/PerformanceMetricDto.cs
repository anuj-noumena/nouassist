using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance.Dtos
{
    /// <summary>
    /// DTO for performance metric
    /// </summary>
    public class PerformanceMetricDto
    {
        /// <summary>
        /// Name of the metric
        /// </summary>
        public string MetricName { get; set; }
        
        /// <summary>
        /// Value of the metric
        /// </summary>
        public double Value { get; set; }
        
        /// <summary>
        /// Unit of measurement
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// Category of the metric
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Formatted value with unit
        /// </summary>
        public string FormattedValue { get; set; }
    }
}
