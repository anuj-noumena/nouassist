namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a competitor that already exists
    /// </summary>
    public class DuplicateCompetitorException : MarketIntelligenceDomainException
    {
        public DuplicateCompetitorException(string competitorName)
            : base("MarketIntelligence:ErrorMessage:DuplicateCompetitor")
        {
            WithData("competitorName", competitorName);
        }

        public DuplicateCompetitorException(string competitorName, string website)
            : base("MarketIntelligence:ErrorMessage:DuplicateCompetitorWebsite")
        {
            WithData("competitorName", competitorName);
            WithData("website", website);
        }
    }
}
