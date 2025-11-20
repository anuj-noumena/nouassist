namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a duplicate metric definition
    /// </summary>
    public class DuplicateMetricDefinitionException : PerformanceFinancialModelingDomainException
    {
        public DuplicateMetricDefinitionException(string metricName)
            : base("PerformanceFinancialModeling:ErrorMessage:DuplicateMetricDefinition")
        {
            WithData("metricName", metricName);
        }
    }
}
