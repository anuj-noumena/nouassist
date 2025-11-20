namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services
{
    /// <summary>
    /// Interface for market opportunity alerting domain service
    /// </summary>
    public interface IOpportunityAlertingService
    {
        /// <summary>
        /// Scans for new market opportunities
        /// </summary>
        Task<List<OpportunityAlert>> ScanForOpportunitiesAsync();

        /// <summary>
        /// Analyzes competitor activities for threats
        /// </summary>
        Task<List<ThreatAlert>> AnalyzeCompetitorThreatsAsync();

        /// <summary>
        /// Generates strategic recommendations
        /// </summary>
        Task<List<StrategicRecommendation>> GenerateStrategicRecommendationsAsync();

        /// <summary>
        /// Publishes opportunity alert event
        /// </summary>
        Task PublishOpportunityAlertAsync(OpportunityAlert alert);
    }
}
