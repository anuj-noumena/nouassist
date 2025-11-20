using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents a single performance data point
    /// </summary>
    public class PerformanceDataPoint : Entity
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
        /// Timestamp of the data point
        /// </summary>
        public DateTime Timestamp { get; protected set; }
        
        /// <summary>
        /// Optional context for the data point
        /// </summary>
        public string Context { get; protected set; }

        protected PerformanceDataPoint()
        {
        }

        /// <summary>
        /// Creates a new performance data point
        /// </summary>
        public PerformanceDataPoint(
            string metricName,
            double value,
            DateTime timestamp,
            string context = null)
        {
            MetricName = metricName;
            Value = value;
            Timestamp = timestamp;
            Context = context;
        }

        /// <summary>
        /// Gets the entity keys
        /// </summary>
        public override object[] GetKeys()
        {
            return new object[] { MetricName, Timestamp };
        }
    }
}
