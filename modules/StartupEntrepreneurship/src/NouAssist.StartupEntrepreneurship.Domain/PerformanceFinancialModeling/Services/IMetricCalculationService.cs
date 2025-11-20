namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Service for calculating metric values based on rules
    /// </summary>
    public interface IMetricCalculationService
    {
        Task<MetricValue> CalculateMetricAsync(TrackedMetric metric, CalculationRule rule, 
                                             Dictionary<string, decimal> inputValues);
        Task<bool> CanCalculateAsync(TrackedMetric metric, CalculationRule rule);
        Task<Dictionary<string, decimal>> GetRequiredMetricValuesAsync(CalculationRule rule, 
                                                                      DateTime? asOfDate = null);
        Task<decimal> ApplyCalculationRuleAsync(CalculationRule rule, Dictionary<string, decimal> values);
    }
}
