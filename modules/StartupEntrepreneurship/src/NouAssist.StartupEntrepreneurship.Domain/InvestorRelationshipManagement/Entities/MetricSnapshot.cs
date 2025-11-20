namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities
{
    /// <summary>
    /// Entity representing a snapshot of metrics for an update
    /// </summary>
    public class MetricSnapshot : Entity<Guid>
    {
        public Guid UpdateId { get; private set; }
        public MetricValue Metric { get; private set; }
        public DateTime SnapshotDate { get; private set; }

        protected MetricSnapshot() { }

        internal MetricSnapshot(
            Guid id,
            Guid updateId,
            MetricValue metric,
            DateTime snapshotDate) : base(id)
        {
            UpdateId = updateId;
            Metric = Check.NotNull(metric, nameof(metric));
            SnapshotDate = snapshotDate;
        }

        public void UpdateMetric(MetricValue metric)
        {
            Metric = Check.NotNull(metric, nameof(metric));
        }
    }
}
