namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Domain service for calculating metric values
    /// </summary>
    public class MetricCalculationService : DomainService, IMetricCalculationService
    {
        private readonly ILogger<MetricCalculationService> _logger;

        public MetricCalculationService(ILogger<MetricCalculationService> logger)
        {
            _logger = logger;
        }

        public async Task<MetricValue> CalculateMetricAsync(TrackedMetric metric, CalculationRule rule, 
                                                          Dictionary<string, decimal> inputValues)
        {
            if (!await CanCalculateAsync(metric, rule))
            {
                throw new InvalidMetricCalculationException(metric.Name, "Missing required input values");
            }

            try
            {
                var calculatedValue = await ApplyCalculationRuleAsync(rule, inputValues);
                return new MetricValue(calculatedValue, metric.DataType, metric.Unit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate metric {MetricName} using rule {Rule}", 
                               metric.Name, rule.Formula);
                throw new InvalidMetricCalculationException(metric.Name, ex.Message);
            }
        }

        public async Task<bool> CanCalculateAsync(TrackedMetric metric, CalculationRule rule)
        {
            if (!rule.IsAutomated) return false;

            var requiredValues = await GetRequiredMetricValuesAsync(rule);
            return rule.CanCalculate(requiredValues);
        }

        public async Task<Dictionary<string, decimal>> GetRequiredMetricValuesAsync(CalculationRule rule, 
                                                                                   DateTime? asOfDate = null)
        {
            // This would typically query the repository for the required metric values
            // For now, returning an empty dictionary as this is domain logic
            await Task.CompletedTask;
            return new Dictionary<string, decimal>();
        }

        public async Task<decimal> ApplyCalculationRuleAsync(CalculationRule rule, Dictionary<string, decimal> values)
        {
            await Task.CompletedTask;

            try
            {
                return rule.Calculate(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying calculation rule: {Formula}", rule.Formula);
                throw;
            }
        }
    }
}
