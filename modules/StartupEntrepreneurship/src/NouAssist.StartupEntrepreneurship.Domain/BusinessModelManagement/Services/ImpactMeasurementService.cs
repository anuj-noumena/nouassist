namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services
{
    /// <summary>
    /// Domain service for measuring and analyzing impact of strategic changes
    /// </summary>
    public class ImpactMeasurementService : DomainService, IImpactMeasurementService
    {
        private readonly IStrategicPivotRepository _pivotRepository;
        private readonly ILogger<ImpactMeasurementService> _logger;

        public ImpactMeasurementService(
            IStrategicPivotRepository pivotRepository,
            ILogger<ImpactMeasurementService> logger)
        {
            _pivotRepository = pivotRepository;
            _logger = logger;
        }

        public async Task<ImpactMetricDefinition> DefineMetricAsync(
            StrategicPivot pivot,
            string metricName,
            string description,
            string unit,
            string measurementMethod,
            decimal? baselineValue = null,
            decimal? targetValue = null)
        {
            try
            {
                var metric = pivot.DefineImpactMetric(
                    metricName,
                    description,
                    unit,
                    measurementMethod,
                    baselineValue,
                    targetValue
                );

                await _pivotRepository.UpdateAsync(pivot);

                _logger.LogInformation("Impact metric defined: {MetricId} - {MetricName} for pivot {PivotId}",
                    metric.Id, metric.MetricName, pivot.Id);

                return metric;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to define impact metric {MetricName} for pivot {PivotId}",
                    metricName, pivot.Id);
                throw;
            }
        }

        public async Task RecordMeasurementAsync(
            ImpactMetricDefinition metric,
            decimal value,
            DateTime measuredAt,
            string measurementMethod = null,
            string notes = null)
        {
            try
            {
                var impact = new ImpactValue(
                    metric.MetricName,
                    value,
                    metric.Unit,
                    measuredAt,
                    measurementMethod ?? metric.MeasurementMethod,
                    notes
                );

                metric.AddMeasurement(impact);

                _logger.LogInformation("Impact measurement recorded: {MetricName} = {Value} {Unit} on {Date}",
                    metric.MetricName, value, metric.Unit, measuredAt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record measurement for metric {MetricId}",
                    metric.Id);
                throw;
            }
        }

        public async Task<Dictionary<string, object>> AnalyzeImpactAsync(StrategicPivot pivot)
        {
            var analysis = new Dictionary<string, object>();

            var metricsWithData = pivot.ImpactMetrics.Where(im => im.Measurements.Any()).ToList();

            analysis["TotalMetricsDefined"] = pivot.ImpactMetrics.Count;
            analysis["MetricsWithData"] = metricsWithData.Count;
            analysis["OverallSuccessRate"] = pivot.CalculateOverallSuccessRate();

            var metricAnalysis = new List<object>();
            foreach (var metric in metricsWithData)
            {
                var measurements = metric.Measurements.OrderBy(m => m.MeasuredAt).ToList();
                var latestValue = measurements.LastOrDefault()?.Value;
                var progress = metric.CalculateProgressToTarget();

                metricAnalysis.Add(new
                {
                    MetricName = metric.MetricName,
                    BaselineValue = metric.BaselineValue,
                    TargetValue = metric.TargetValue,
                    LatestValue = latestValue,
                    ProgressToTarget = progress,
                    TrendDirection = CalculateTrendDirection(measurements),
                    MeasurementCount = measurements.Count
                });
            }

            analysis["MetricAnalysis"] = metricAnalysis;

            // Overall pivot performance
            analysis["PivotStatus"] = pivot.Status.ToString();
            analysis["IsOverdue"] = pivot.IsOverdue();
            analysis["Duration"] = pivot.GetDuration()?.TotalDays;

            return analysis;
        }

        public async Task<List<ImpactValue>> GetTrendDataAsync(
            ImpactMetricDefinition metric,
            DateTime fromDate,
            DateTime toDate)
        {
            return metric.Measurements
                .Where(m => m.MeasuredAt >= fromDate && m.MeasuredAt <= toDate)
                .OrderBy(m => m.MeasuredAt)
                .ToList();
        }

        public async Task<ValidationResult> ValidateMetricDefinitionAsync(
            string metricName,
            string unit,
            string measurementMethod)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(metricName))
                result.AddError("Metric name is required");

            if (string.IsNullOrWhiteSpace(unit))
                result.AddError("Unit is required");

            if (string.IsNullOrWhiteSpace(measurementMethod))
                result.AddError("Measurement method is required");

            // Check for standard metric naming conventions
            if (!string.IsNullOrWhiteSpace(metricName))
            {
                if (metricName.ToLower().Contains("rate") && !unit.ToLower().Contains("percent"))
                    result.AddWarning("Rates should typically be measured in percentages");

                if (metricName.ToLower().Contains("cost") && !unit.ToLower().Contains("currency"))
                    result.AddWarning("Cost metrics should typically be measured in currency units");

                if (metricName.ToLower().Contains("time") && !unit.ToLower().Contains("day") && 
                    !unit.ToLower().Contains("hour") && !unit.ToLower().Contains("minute"))
                    result.AddWarning("Time metrics should be measured in time units");
            }

            return result;
        }

        private string CalculateTrendDirection(List<ImpactValue> measurements)
        {
            if (measurements.Count < 2)
                return "Insufficient Data";

            var firstValue = measurements.First().Value;
            var lastValue = measurements.Last().Value;

            if (lastValue > firstValue)
                return "Increasing";
            else if (lastValue < firstValue)
                return "Decreasing";
            else
                return "Stable";
        }
    }
}
