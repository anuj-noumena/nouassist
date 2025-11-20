namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories
{
    /// <summary>
    /// Repository interface for Competitor aggregate
    /// </summary>
    public interface ICompetitorRepository : IRepository<Competitor, Guid>
    {
        /// <summary>
        /// Finds a competitor by name
        /// </summary>
        Task<Competitor> FindByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a competitor with the given name already exists
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a competitor with the given website already exists
        /// </summary>
        Task<bool> ExistsByWebsiteAsync(string website, Guid? excludeId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets competitors by threat level
        /// </summary>
        Task<List<Competitor>> GetByThreatLevelAsync(
            CompetitorThreatLevel threatLevel, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets competitors with recent updates
        /// </summary>
        Task<List<Competitor>> GetWithRecentUpdatesAsync(
            int days = 30, 
            int maxResultCount = 10, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets competitors by industry
        /// </summary>
        Task<List<Competitor>> GetByIndustryAsync(
            string industry, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets competitors by specification
        /// </summary>
        Task<List<Competitor>> GetListAsync(
            ISpecification<Competitor> specification,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);
    }
}
