using Volo.Abp;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions
{
    /// <summary>
    /// Base exception for AnalyticsReporting domain
    /// </summary>
    public class AnalyticsReportingDomainException : BusinessException
    {
        public AnalyticsReportingDomainException(string code)
            : base(code)
        {
        }

        public AnalyticsReportingDomainException(string code, string message)
            : base(code, message)
        {
        }

        public AnalyticsReportingDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
