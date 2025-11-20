namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Exceptions
{
    /// <summary>
    /// Exception thrown when a metric is not found
    /// </summary>
    public class MetricNotFoundException : PerformanceFinancialModelingDomainException
    {
        public MetricNotFoundException(Guid metricId)
            : base("PerformanceFinancialModeling:ErrorMessage:MetricNotFound")
        {
            WithData("metricId", metricId);
        }
        
        public MetricNotFoundException(string metricName)
            : base("PerformanceFinancialModeling:ErrorMessage:MetricNotFoundByName")
        {
            WithData("metricName", metricName);
        }
    }
}
