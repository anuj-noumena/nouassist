namespace NouAssist.RequirementsManagement.QualityAssurance.Entities
{
    /// <summary>
    /// Represents a snapshot of a quality metric at a point in time
    /// </summary>
    public class QualityMetricSnapshot : Entity<Guid>
    {
        /// <summary>
        /// The tenant that this metric belongs to
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// The metric value
        /// </summary>
        public MetricValue Metric { get; private set; }
        
        /// <summary>
        /// Optional ID of the test plan this metric is associated with
        /// </summary>
        public Guid? TestPlanId { get; private set; }
        
        /// <summary>
        /// Optional release version this metric is associated with
        /// </summary>
        public string ReleaseVersion { get; private set; }
        
        /// <summary>
        /// Additional contextual information about the metric
        /// </summary>
        public string Context { get; private set; }

        protected QualityMetricSnapshot()
        {
        }

        public QualityMetricSnapshot(
            Guid id,
            MetricValue metric,
            Guid? tenantId = null,
            Guid? testPlanId = null,
            string releaseVersion = null,
            string context = null) : base(id)
        {
            Metric = metric;
            TenantId = tenantId;
            TestPlanId = testPlanId;
            ReleaseVersion = releaseVersion;
            Context = context;
        }
    }
}
