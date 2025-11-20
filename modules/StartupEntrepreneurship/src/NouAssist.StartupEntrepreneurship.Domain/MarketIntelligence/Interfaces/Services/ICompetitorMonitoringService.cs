namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services
{
    /// <summary>
    /// Interface for competitor monitoring domain service
    /// </summary>
    public interface ICompetitorMonitoringService
    {
        /// <summary>
        /// Creates a new competitor
        /// </summary>
        Task<Competitor> CreateCompetitorAsync(
            string name,
            string description,
            CompetitorInfo competitorInfo,
            CompetitorThreatLevel threatLevel = CompetitorThreatLevel.Low);

        /// <summary>
        /// Updates a competitor's information
        /// </summary>
        Task<Competitor> UpdateCompetitorInfoAsync(
            Guid competitorId,
            string name,
            string description,
            CompetitorInfo competitorInfo);

        /// <summary>
        /// Adds a competitor update log
        /// </summary>
        Task AddCompetitorUpdateAsync(
            Guid competitorId,
            UpdateType updateType,
            string title,
            string description,
            ImpactLevel impactLevel = ImpactLevel.Low,
            string source = null,
            string tags = null);

        /// <summary>
        /// Changes a competitor's threat level
        /// </summary>
        Task ChangeCompetitorThreatLevelAsync(
            Guid competitorId,
            CompetitorThreatLevel newThreatLevel,
            string reason = null);

        /// <summary>
        /// Evaluates competitors and updates competitive scores
        /// </summary>
        Task EvaluateCompetitorsAsync();

        /// <summary>
        /// Gets competitors sorted by threat level
        /// </summary>
        Task<List<Competitor>> GetCompetitorsByThreatLevelAsync();

        /// <summary>
        /// Gets competitors with recent updates
        /// </summary>
        Task<List<Competitor>> GetCompetitorsWithRecentUpdatesAsync(int days = 30, int maxResultCount = 10);

        /// <summary>
        /// Archives a competitor
        /// </summary>
        Task ArchiveCompetitorAsync(Guid competitorId);
    }
}
