using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents a performance metric value in a snapshot
    /// </summary>
    public class PerformanceMetricValue : Entity
    {
        /// <summary>
        /// Name of the metric
        /// </summary>
        public string MetricName { get; protected set; }
        
        /// <summary>
        /// Value of the metric
        /// </summary>
        public double Value { get; protected set; }
        
        /// <summary>
        /// Unit of measurement (e.g., "%", "hours", "count")
        /// </summary>
        public string Unit { get; protected set; }
        
        /// <summary>
        /// Category of the metric (e.g., "Productivity", "Focus", "Efficiency")
        /// </summary>
        public string Category { get; protected set; }

        protected PerformanceMetricValue()
        {
        }

        /// <summary>
        /// Creates a new performance metric value
        /// </summary>
        public PerformanceMetricValue(
            string metricName,
            double value,
            string unit = null,
            string category = null)
        {
            MetricName = metricName;
            Value = value;
            Unit = unit;
            Category = category;
        }

        /// <summary>
        /// Gets the entity keys
        /// </summary>
        public override object[] GetKeys()
        {
            return new object[] { MetricName };
        }
    }
}
