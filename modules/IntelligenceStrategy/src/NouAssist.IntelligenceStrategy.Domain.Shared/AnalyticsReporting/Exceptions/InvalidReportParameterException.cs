using System;

namespace NouAssist.Analytics.Exceptions
{
    /// <summary>
    /// Exception thrown when a report parameter is invalid
    /// </summary>
    public class InvalidReportParameterException : AnalyticsDomainException
    {
        public Guid ReportConfigurationId { get; }
        public string ParameterName { get; }
        
        public InvalidReportParameterException(Guid reportConfigurationId, string parameterName, string reason)
            : base("Analytics:ErrorMessage:InvalidReportParameter")
        {
            ReportConfigurationId = reportConfigurationId;
            ParameterName = parameterName;
            WithData("reportConfigurationId", reportConfigurationId);
            WithData("parameterName", parameterName);
            WithData("reason", reason);
        }
    }
}
