namespace NouAssist.IntelligenceStrategy.WorkflowEngine.ValueObjects
{
    /// <summary>
    /// Value object representing performance metrics for a workflow
    /// </summary>
    public class PerformanceMetrics : ValueObject
    {
        /// <summary>
        /// The average cycle time in minutes
        /// </summary>
        public double AverageCycleTimeMinutes { get; private set; }
        
        /// <summary>
        /// The bottleneck score (0-100, higher means more severe bottleneck)
        /// </summary>
        public int BottleneckScore { get; private set; }
        
        /// <summary>
        /// The ID of the activity that is the bottleneck
        /// </summary>
        public Guid? BottleneckActivityId { get; private set; }
        
        /// <summary>
        /// The number of instances used to calculate these metrics
        /// </summary>
        public int SampleSize { get; private set; }
        
        /// <summary>
        /// The timestamp when the metrics were calculated
        /// </summary>
        public DateTime CalculationTime { get; private set; }

        protected PerformanceMetrics() { }

        public PerformanceMetrics(
            double averageCycleTimeMinutes,
            int bottleneckScore,
            Guid? bottleneckActivityId,
            int sampleSize)
        {
            AverageCycleTimeMinutes = averageCycleTimeMinutes;
            BottleneckScore = bottleneckScore;
            BottleneckActivityId = bottleneckActivityId;
            SampleSize = sampleSize;
            CalculationTime = DateTime.UtcNow;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return AverageCycleTimeMinutes;
            yield return BottleneckScore;
            yield return BottleneckActivityId;
            yield return SampleSize;
            yield return CalculationTime;
        }
    }
}
