using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Learning.ValueObjects
{
    /// <summary>
    /// Represents calculated metrics for a learning topic
    /// </summary>
    public class TopicMetrics : ValueObject
    {
        public int TotalTimeSpentMs { get; init; }
        public int EstimatedTimeRemainingMs { get; init; }
        public double CompletionPercentage { get; init; }
        public int ActiveChildCount { get; init; }
        public int CompletedChildCount { get; init; }
        public double AverageProgressOfChildren { get; init; }

        public TopicMetrics(
            int totalTimeSpentMs,
            int estimatedTimeRemainingMs,
            double completionPercentage,
            int activeChildCount,
            int completedChildCount,
            double averageProgressOfChildren)
        {
            TotalTimeSpentMs = totalTimeSpentMs;
            EstimatedTimeRemainingMs = estimatedTimeRemainingMs;
            CompletionPercentage = completionPercentage;
            ActiveChildCount = activeChildCount;
            CompletedChildCount = completedChildCount;
            AverageProgressOfChildren = averageProgressOfChildren;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TotalTimeSpentMs;
            yield return EstimatedTimeRemainingMs;
            yield return CompletionPercentage;
            yield return ActiveChildCount;
            yield return CompletedChildCount;
            yield return AverageProgressOfChildren;
        }
    }
}
