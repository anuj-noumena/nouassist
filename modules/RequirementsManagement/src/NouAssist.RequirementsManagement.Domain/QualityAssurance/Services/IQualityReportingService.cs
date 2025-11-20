namespace NouAssist.RequirementsManagement.QualityAssurance.Services
{
    /// <summary>
    /// Domain service for quality reporting
    /// </summary>
    public interface IQualityReportingService
    {
        /// <summary>
        /// Creates a new quality metric snapshot
        /// </summary>
        Task<QualityMetricSnapshot> CreateMetricSnapshotAsync(
            MetricType type,
            decimal value,
            string unit,
            Guid? testPlanId = null,
            string releaseVersion = null,
            string context = null);
        
        /// <summary>
        /// Gets test coverage metrics for a release version
        /// </summary>
        Task<List<QualityMetricSnapshot>> GetTestCoverageMetricsAsync(string releaseVersion);
        
        /// <summary>
        /// Gets defect metrics for a release version
        /// </summary>
        Task<List<QualityMetricSnapshot>> GetDefectMetricsAsync(string releaseVersion);
        
        /// <summary>
        /// Calculates test pass rate for a test plan
        /// </summary>
        Task<decimal> CalculateTestPassRateAsync(Guid testPlanId);
        
        /// <summary>
        /// Calculates defect density for a release version
        /// </summary>
        Task<decimal> CalculateDefectDensityAsync(string releaseVersion, int codeSize);
        
        /// <summary>
        /// Creates a metric snapshot for test pass rate
        /// </summary>
        Task<QualityMetricSnapshot> CaptureTestPassRateAsync(Guid testPlanId);
        
        /// <summary>
        /// Creates a metric snapshot for open defects count
        /// </summary>
        Task<QualityMetricSnapshot> CaptureOpenDefectsAsync(string releaseVersion);
    }
}
