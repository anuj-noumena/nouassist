namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects
{
    /// <summary>
    /// Represents an impact measurement value with metadata
    /// </summary>
    public class ImpactValue : ValueObject
    {
        public string MetricName { get; private set; }
        public decimal Value { get; private set; }
        public string Unit { get; private set; }
        public DateTime MeasuredAt { get; private set; }
        public string MeasurementMethod { get; private set; }
        public string Notes { get; private set; }

        protected ImpactValue() { }

        public ImpactValue(
            string metricName,
            decimal value,
            string unit,
            DateTime measuredAt,
            string measurementMethod = null,
            string notes = null)
        {
            SetMetricName(metricName);
            Value = value;
            SetUnit(unit);
            MeasuredAt = measuredAt;
            MeasurementMethod = measurementMethod;
            Notes = notes;
        }

        private void SetMetricName(string metricName)
        {
            MetricName = Check.NotNullOrWhiteSpace(metricName, nameof(metricName), maxLength: 100);
        }

        private void SetUnit(string unit)
        {
            Unit = Check.NotNullOrWhiteSpace(unit, nameof(unit), maxLength: 50);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MetricName;
            yield return Value;
            yield return Unit;
            yield return MeasuredAt.Date;
            yield return MeasurementMethod;
            yield return Notes;
        }
    }
}
