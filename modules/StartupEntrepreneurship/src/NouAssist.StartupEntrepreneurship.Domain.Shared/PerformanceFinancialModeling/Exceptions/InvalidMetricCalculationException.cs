namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Exceptions
{
    /// <summary>
    /// Exception thrown when metric calculation fails
    /// </summary>
    public class InvalidMetricCalculationException : PerformanceFinancialModelingDomainException
    {
        public InvalidMetricCalculationException(string metricName, string reason)
            : base("PerformanceFinancialModeling:ErrorMessage:InvalidCalculation")
        {
            WithData("metricName", metricName);
            WithData("reason", reason);
        }
    }
}
