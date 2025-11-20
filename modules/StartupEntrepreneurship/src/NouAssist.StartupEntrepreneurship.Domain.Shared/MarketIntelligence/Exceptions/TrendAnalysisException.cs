namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Exceptions
{
    /// <summary>
    /// Exception thrown during trend analysis operations
    /// </summary>
    public class TrendAnalysisException : MarketIntelligenceDomainException
    {
        public TrendAnalysisException(string operation, string reason)
            : base("MarketIntelligence:ErrorMessage:TrendAnalysisError")
        {
            WithData("operation", operation);
            WithData("reason", reason);
        }
    }
}
