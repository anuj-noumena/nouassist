using System;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.Common
{
    /// <summary>
    /// DTO for performance metrics data
    /// </summary>
    public class PerformanceMetricsDto
    {
        /// <summary>
        /// The average cycle time in minutes
        /// </summary>
        public double AverageCycleTimeMinutes { get; set; }
        
        /// <summary>
        /// The bottleneck score (0-100, higher means more severe bottleneck)
        /// </summary>
        public int BottleneckScore { get; set; }
        
        /// <summary>
        /// The ID of the activity that is the bottleneck
        /// </summary>
        public Guid? BottleneckActivityId { get; set; }
        
        /// <summary>
        /// The name of the bottleneck activity, if known
        /// </summary>
        public string BottleneckActivityName { get; set; }
        
        /// <summary>
        /// The number of instances used to calculate these metrics
        /// </summary>
        public int SampleSize { get; set; }
        
        /// <summary>
        /// The timestamp when the metrics were calculated
        /// </summary>
        public DateTime CalculationTime { get; set; }
    }
}
