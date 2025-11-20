using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects
{
    /// <summary>
    /// Represents a value for a KPI at a specific point in time
    /// </summary>
    public class KpiValue : ValueObject
    {
        /// <summary>
        /// The numeric value of the KPI
        /// </summary>
        public decimal Value { get; private set; }
        
        /// <summary>
        /// The timestamp when this value was measured
        /// </summary>
        public DateTime Timestamp { get; private set; }
        
        /// <summary>
        /// The status of the KPI based on thresholds
        /// </summary>
        public KpiStatus Status { get; private set; }
        
        /// <summary>
        /// Optional formatted display value
        /// </summary>
        public string FormattedValue { get; private set; }
        
        private KpiValue()
        {
        }
        
        public KpiValue(decimal value, DateTime timestamp, KpiStatus status, string formattedValue = null)
        {
            Value = value;
            Timestamp = timestamp;
            Status = status;
            FormattedValue = formattedValue;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Timestamp;
            yield return Status;
            yield return FormattedValue ?? string.Empty;
        }
    }
}
