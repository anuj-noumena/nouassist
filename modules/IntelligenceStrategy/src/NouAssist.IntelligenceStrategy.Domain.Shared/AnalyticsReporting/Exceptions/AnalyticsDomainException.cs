using Volo.Abp;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions
{
    /// <summary>
    /// Base exception for Analytics domain
    /// </summary>
    public class AnalyticsDomainException : BusinessException
    {
        public AnalyticsDomainException(string code)
            : base(code)
        {
        }

        public AnalyticsDomainException(string code, string message)
            : base(code, message)
        {
        }

        public AnalyticsDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
