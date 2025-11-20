namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Exceptions
{
    /// <summary>
    /// Base exception for Market Intelligence domain
    /// </summary>
    public class MarketIntelligenceDomainException : BusinessException
    {
        public MarketIntelligenceDomainException(string code)
            : base(code)
        {
        }

        public MarketIntelligenceDomainException(string code, string message)
            : base(code, message)
        {
        }

        public MarketIntelligenceDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
