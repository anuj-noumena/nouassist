namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories
{
    /// <summary>
    /// Repository interface for MarketTrend aggregate
    /// </summary>
    public interface IMarketTrendRepository : IRepository<MarketTrend, Guid>
    {
        /// <summary>
        /// Gets active market trends
        /// </summary>
        Task<List<MarketTrend>> GetActiveAsync(bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets market trends by category
        /// </summary>
        Task<List<MarketTrend>> GetByCategoryAsync(
            TrendCategory category,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets market trends by impact level
        /// </summary>
        Task<List<MarketTrend>> GetByImpactLevelAsync(
            ImpactLevel impactLevel,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets market trends by keywords
        /// </summary>
        Task<List<MarketTrend>> GetByKeywordsAsync(
            string keywords,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets high-confidence trends
        /// </summary>
        Task<List<MarketTrend>> GetHighConfidenceTrendsAsync(
            decimal minConfidenceScore = 0.8m,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets trends by specification
        /// </summary>
        Task<List<MarketTrend>> GetListAsync(
            ISpecification<MarketTrend> specification,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);
    }
}
