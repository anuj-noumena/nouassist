using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Exceptions
{
    /// <summary>
    /// Exception thrown when SWOT item validation fails
    /// </summary>
    public class InvalidSwotItemException : MarketIntelligenceDomainException
    {
        public InvalidSwotItemException(SwotCategory category, string reason)
            : base("MarketIntelligence:ErrorMessage:InvalidSwotItem")
        {
            WithData("category", category);
            WithData("reason", reason);
        }
    }
}
