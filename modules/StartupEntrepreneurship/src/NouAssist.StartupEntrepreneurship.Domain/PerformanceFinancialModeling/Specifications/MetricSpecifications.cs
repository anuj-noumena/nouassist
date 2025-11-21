using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Specifications
{
    /// <summary>
    /// Base specification class for TrackedMetric aggregate
    /// </summary>
    public abstract class TrackedMetricSpecification : Specification<TrackedMetric>
    {
    }

    /// <summary>
    /// Specification for active metrics of a specific type
    /// </summary>
    public class ActiveMetricsByTypeSpecification : TrackedMetricSpecification
    {
        private readonly MetricType _metricType;

        public ActiveMetricsByTypeSpecification(MetricType metricType)
        {
            _metricType = metricType;
        }

        public override Expression<Func<TrackedMetric, bool>> ToExpression()
        {
            return metric => metric.IsActive && metric.Type == _metricType;
        }
    }

    /// <summary>
    /// Specification for metrics with recent data points
    /// </summary>
    public class MetricsWithRecentDataSpecification : TrackedMetricSpecification
    {
        private readonly DateTime _cutoffDate;

        public MetricsWithRecentDataSpecification(int daysBack = 30)
        {
            _cutoffDate = DateTime.UtcNow.AddDays(-daysBack);
        }

        public override Expression<Func<TrackedMetric, bool>> ToExpression()
        {
            return metric => metric.IsActive && 
                           metric.LastUpdatedAt.HasValue && 
                           metric.LastUpdatedAt.Value >= _cutoffDate;
        }
    }

    /// <summary>
    /// Specification for metrics with active goals
    /// </summary>
    public class MetricsWithActiveGoalsSpecification : TrackedMetricSpecification
    {
        public override Expression<Func<TrackedMetric, bool>> ToExpression()
        {
            return metric => metric.IsActive && 
                           metric.Goals.Any(g => g.IsActive && !g.Target.IsOverdue());
        }
    }

    /// <summary>
    /// Specification for metrics by frequency
    /// </summary>
    public class MetricsByFrequencySpecification : TrackedMetricSpecification
    {
        private readonly MetricFrequency _frequency;

        public MetricsByFrequencySpecification(MetricFrequency frequency)
        {
            _frequency = frequency;
        }

        public override Expression<Func<TrackedMetric, bool>> ToExpression()
        {
            return metric => metric.IsActive && metric.Frequency == _frequency;
        }
    }

    /// <summary>
    /// Specification for metrics due for update based on frequency
    /// </summary>
    public class MetricsDueForUpdateSpecification : TrackedMetricSpecification
    {
        public override Expression<Func<TrackedMetric, bool>> ToExpression()
        {
            var now = DateTime.UtcNow;
            
            return metric => metric.IsActive && 
                           (metric.LastUpdatedAt == null || 
                            (metric.Frequency == MetricFrequency.Daily && metric.LastUpdatedAt.Value.Date < now.Date) ||
                            (metric.Frequency == MetricFrequency.Weekly && metric.LastUpdatedAt.Value < now.AddDays(-7)) ||
                            (metric.Frequency == MetricFrequency.Monthly && metric.LastUpdatedAt.Value < now.AddMonths(-1)) ||
                            (metric.Frequency == MetricFrequency.Quarterly && metric.LastUpdatedAt.Value < now.AddMonths(-3)) ||
                            (metric.Frequency == MetricFrequency.Annually && metric.LastUpdatedAt.Value < now.AddYears(-1)));
        }
    }
}
