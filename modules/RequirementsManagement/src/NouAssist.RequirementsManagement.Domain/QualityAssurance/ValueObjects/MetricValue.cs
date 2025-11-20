namespace NouAssist.RequirementsManagement.QualityAssurance.ValueObjects
{
    /// <summary>
    /// Represents a quality metric value with type and timestamp
    /// </summary>
    public class MetricValue : ValueObject
    {
        /// <summary>
        /// The type of metric being measured
        /// </summary>
        public MetricType Type { get; private set; }
        
        /// <summary>
        /// The numeric value of the metric
        /// </summary>
        public decimal Value { get; private set; }
        
        /// <summary>
        /// Unit of measurement (e.g., percentage, count, hours)
        /// </summary>
        public string Unit { get; private set; }
        
        /// <summary>
        /// When the metric was captured
        /// </summary>
        public DateTime Timestamp { get; private set; }

        protected MetricValue() { }

        public MetricValue(MetricType type, decimal value, string unit, DateTime timestamp)
        {
            Type = type;
            Value = value;
            Unit = Check.NotNullOrWhiteSpace(unit, nameof(unit));
            Timestamp = timestamp;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Value;
            yield return Unit;
            yield return Timestamp;
        }
    }
}
