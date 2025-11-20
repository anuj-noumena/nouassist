namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Interfaces.Services
{
    /// <summary>
    /// Defines methods for managing portfolio roadmaps
    /// </summary>
    public interface IPortfolioRoadmapManager
    {
        /// <summary>
        /// Creates a new portfolio roadmap
        /// </summary>
        Task<PortfolioRoadmapAggregate> CreateRoadmapAsync(
            string title,
            string description,
            Guid portfolioId,
            DateTime startDate,
            DateTime endDate,
            Guid? tenantId = null);

        /// <summary>
        /// Adds a milestone to a roadmap
        /// </summary>
        Task<RoadmapMilestone> AddRoadmapMilestoneAsync(
            Guid roadmapId,
            string title,
            string description,
            DateTime plannedDate,
            Guid? portfolioItemId = null);

        /// <summary>
        /// Creates a dependency between milestones
        /// </summary>
        Task<CrossItemDependency> CreateMilestoneDependencyAsync(
            Guid roadmapId,
            Guid sourceMilestoneId,
            Guid targetMilestoneId,
            DependencyType dependencyType);

        /// <summary>
        /// Updates a milestone's completion status
        /// </summary>
        Task UpdateMilestoneCompletionAsync(
            Guid roadmapId,
            Guid milestoneId,
            bool isCompleted,
            DateTime? completionDate = null);

        /// <summary>
        /// Gets milestones within a specific date range
        /// </summary>
        Task<List<RoadmapMilestone>> GetMilestonesInDateRangeAsync(
            Guid roadmapId,
            DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Gets dependencies for a milestone
        /// </summary>
        Task<List<CrossItemDependency>> GetDependenciesForMilestoneAsync(
            Guid roadmapId,
            Guid milestoneId);
    }
}
