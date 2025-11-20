namespace NouAssist.RequirementsManagement.QualityAssurance.Services
{
    /// <summary>
    /// Implementation of quality reporting domain service
    /// </summary>
    public class QualityReportingService : DomainService, IQualityReportingService
    {
        private readonly IQualityMetricSnapshotRepository _metricRepository;
        private readonly ITestPlanRepository _testPlanRepository;
        private readonly IDefectReportRepository _defectRepository;
        private readonly ILogger<QualityReportingService> _logger;

        public QualityReportingService(
            IQualityMetricSnapshotRepository metricRepository,
            ITestPlanRepository testPlanRepository,
            IDefectReportRepository defectRepository,
            ILogger<QualityReportingService> logger)
        {
            _metricRepository = metricRepository;
            _testPlanRepository = testPlanRepository;
            _defectRepository = defectRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<QualityMetricSnapshot> CreateMetricSnapshotAsync(
            MetricType type,
            decimal value,
            string unit,
            Guid? testPlanId = null,
            string releaseVersion = null,
            string context = null)
        {
            try
            {
                var metricValue = new MetricValue(type, value, unit, Clock.Now);
                
                var snapshot = new QualityMetricSnapshot(
                    GuidGenerator.Create(),
                    metricValue,
                    CurrentTenant.Id,
                    testPlanId,
                    releaseVersion,
                    context);
                
                await _metricRepository.InsertAsync(snapshot);
                
                _logger.LogInformation("Created {MetricType} metric snapshot with value {Value} {Unit}", 
                    type, value, unit);
                
                return snapshot;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create {MetricType} metric snapshot", type);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<QualityMetricSnapshot>> GetTestCoverageMetricsAsync(string releaseVersion)
        {
            try
            {
                var metrics = await _metricRepository.GetByReleaseVersionAsync(releaseVersion);
                
                return metrics.Where(m => m.Metric.Type == MetricType.TestCoverage).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get test coverage metrics for release {ReleaseVersion}", 
                    releaseVersion);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<QualityMetricSnapshot>> GetDefectMetricsAsync(string releaseVersion)
        {
            try
            {
                var metrics = await _metricRepository.GetByReleaseVersionAsync(releaseVersion);
                
                return metrics.Where(m => 
                    m.Metric.Type == MetricType.DefectDensity || 
                    m.Metric.Type == MetricType.OpenDefects ||
                    m.Metric.Type == MetricType.CriticalDefects).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get defect metrics for release {ReleaseVersion}", 
                    releaseVersion);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<decimal> CalculateTestPassRateAsync(Guid testPlanId)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetWithDetailsAsync(testPlanId);
                
                var progress = testPlan.GetTestExecutionProgress();
                
                if (progress.Total == 0)
                {
                    return 0;
                }
                
                return (decimal)progress.Passed / progress.Total * 100;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate test pass rate for test plan {TestPlanId}", 
                    testPlanId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<decimal> CalculateDefectDensityAsync(string releaseVersion, int codeSize)
        {
            try
            {
                if (codeSize <= 0)
                {
                    throw new BusinessRuleViolationException("Code size must be greater than zero");
                }
                
                var defects = await _defectRepository.GetAsync(d => d.Version == releaseVersion);
                
                return (decimal)defects.Count / codeSize * 1000; // Defects per 1000 lines of code
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate defect density for release {ReleaseVersion}", 
                    releaseVersion);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<QualityMetricSnapshot> CaptureTestPassRateAsync(Guid testPlanId)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetAsync(testPlanId);
                var passRate = await CalculateTestPassRateAsync(testPlanId);
                
                return await CreateMetricSnapshotAsync(
                    MetricType.TestPassRate,
                    passRate,
                    "percentage",
                    testPlanId,
                    testPlan.ReleaseVersion,
                    $"Test Pass Rate for {testPlan.Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to capture test pass rate for test plan {TestPlanId}", 
                    testPlanId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<QualityMetricSnapshot> CaptureOpenDefectsAsync(string releaseVersion)
        {
            try
            {
                var openDefects = await _defectRepository.GetAsync(d => 
                    d.Version == releaseVersion && 
                    d.Status != DefectStatus.Closed && 
                    d.Status != DefectStatus.CannotReproduce &&
                    d.Status != DefectStatus.NotADefect);
                
                return await CreateMetricSnapshotAsync(
                    MetricType.OpenDefects,
                    openDefects.Count,
                    "count",
                    null,
                    releaseVersion,
                    $"Open Defects for {releaseVersion}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to capture open defects for release {ReleaseVersion}", 
                    releaseVersion);
                throw;
            }
        }
    }
}
