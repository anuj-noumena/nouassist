namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions
{
    /// <summary>
    /// Exception thrown when a KPI configuration is invalid
    /// </summary>
    public class InvalidKpiConfigurationException : AnalyticsReportingDomainException
    {
        public InvalidKpiConfigurationException(string reason)
            : base("AnalyticsReporting:ErrorMessage:InvalidKpiConfiguration")
        {
            WithData("reason", reason);
        }
    }
}
