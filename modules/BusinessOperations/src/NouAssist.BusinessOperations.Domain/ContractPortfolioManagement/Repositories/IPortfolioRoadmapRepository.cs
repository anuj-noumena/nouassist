namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Repositories
{
    /// <summary>
    /// Repository for PortfolioRoadmap aggregate
    /// </summary>
    public interface IPortfolioRoadmapRepository : IRepository<PortfolioRoadmapAggregate, Guid>
    {
        /// <summary>
        /// Gets roadmaps by portfolio ID
        /// </summary>
        Task<List<PortfolioRoadmapAggregate>> GetByPortfolioIdAsync(
            Guid portfolioId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets roadmaps with eager loaded milestones
        /// </summary>
        Task<PortfolioRoadmapAggregate> GetWithMilestonesAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets roadmaps with eager loaded dependencies
        /// </summary>
        Task<PortfolioRoadmapAggregate> GetWithDependenciesAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets roadmaps with all related entities
        /// </summary>
        Task<PortfolioRoadmapAggregate> GetWithAllDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets milestones in a specific date range
        /// </summary>
        Task<List<RoadmapMilestone>> GetMilestonesInDateRangeAsync(
            Guid roadmapId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default);
    }
}
