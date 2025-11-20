namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Exceptions
{
    /// <summary>
    /// Exception thrown when a competitor is not found
    /// </summary>
    public class CompetitorNotFoundException : MarketIntelligenceDomainException
    {
        public CompetitorNotFoundException(Guid competitorId)
            : base("MarketIntelligence:ErrorMessage:CompetitorNotFound")
        {
            WithData("competitorId", competitorId);
        }

        public CompetitorNotFoundException(string competitorName)
            : base("MarketIntelligence:ErrorMessage:CompetitorNotFoundByName")
        {
            WithData("competitorName", competitorName);
        }
    }
}
