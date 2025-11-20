namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions
{
    /// <summary>
    /// Exception thrown when a data source configuration is invalid
    /// </summary>
    public class InvalidDataSourceConfigurationException : AnalyticsReportingDomainException
    {
        public InvalidDataSourceConfigurationException(string reason)
            : base("AnalyticsReporting:ErrorMessage:InvalidDataSourceConfiguration")
        {
            WithData("reason", reason);
        }
    }
}
