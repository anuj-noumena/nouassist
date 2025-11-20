namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for measuring and analyzing impact of strategic changes
    /// </summary>
    public interface IImpactMeasurementService
    {
        Task<ImpactMetricDefinition> DefineMetricAsync(
            StrategicPivot pivot,
            string metricName,
            string description,
            string unit,
            string measurementMethod,
            decimal? baselineValue = null,
            decimal? targetValue = null);

        Task RecordMeasurementAsync(
            ImpactMetricDefinition metric,
            decimal value,
            DateTime measuredAt,
            string measurementMethod = null,
            string notes = null);

        Task<Dictionary<string, object>> AnalyzeImpactAsync(StrategicPivot pivot);

        Task<List<ImpactValue>> GetTrendDataAsync(
            ImpactMetricDefinition metric,
            DateTime fromDate,
            DateTime toDate);

        Task<ValidationResult> ValidateMetricDefinitionAsync(
            string metricName,
            string unit,
            string measurementMethod);
    }
}
