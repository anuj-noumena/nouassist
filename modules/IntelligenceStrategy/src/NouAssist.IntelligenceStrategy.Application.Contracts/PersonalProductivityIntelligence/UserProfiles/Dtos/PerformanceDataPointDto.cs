using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos
{
    /// <summary>
    /// DTO for performance data point
    /// </summary>
    public class PerformanceDataPointDto
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
        /// Timestamp when the data point was recorded
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Optional context information
        /// </summary>
        public string Context { get; set; }
    }
}
