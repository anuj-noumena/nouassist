namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities
{
    /// <summary>
    /// Represents a single data point for a metric
    /// </summary>
    public class MetricDataPoint : Entity
    {
        public Guid MetricId { get; private set; }
        public MetricValue Value { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Notes { get; private set; }

        protected MetricDataPoint() { }

        internal MetricDataPoint(Guid metricId, MetricValue value, DateTime timestamp, string notes = null)
        {
            MetricId = metricId;
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Timestamp = timestamp;
            SetNotes(notes);
        }

        public void UpdateNotes(string notes)
        {
            SetNotes(notes);
        }

        private void SetNotes(string notes)
        {
            if (!string.IsNullOrEmpty(notes))
            {
                Notes = Check.Length(
                    notes.Trim(),
                    nameof(notes),
                    maxLength: PerformanceFinancialModelingConsts.MaxNotesLength
                );
            }
        }

        public override object[] GetKeys()
        {
            return new object[] { MetricId, Timestamp };
        }
    }
}
