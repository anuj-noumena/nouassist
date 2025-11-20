namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories
{
    /// <summary>
    /// Repository for LearningResource aggregate
    /// </summary>
    public interface ILearningResourceRepository : IRepository<LearningResource, Guid>
    {
        /// <summary>
        /// Find resources by specification
        /// </summary>
        Task<List<LearningResource>> GetListAsync(
            Specification<LearningResource> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Count resources matching specification
        /// </summary>
        Task<int> CountAsync(
            Specification<LearningResource> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if any resources match specification
        /// </summary>
        Task<bool> AnyAsync(
            Specification<LearningResource> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get published resources
        /// </summary>
        Task<List<LearningResource>> GetPublishedAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get featured resources
        /// </summary>
        Task<List<LearningResource>> GetFeaturedAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get resources by type
        /// </summary>
        Task<List<LearningResource>> GetByTypeAsync(
            ResourceType resourceType, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get resources completed by user
        /// </summary>
        Task<List<LearningResource>> GetCompletedByUserAsync(
            Guid userId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get resources in progress by user
        /// </summary>
        Task<List<LearningResource>> GetInProgressByUserAsync(
            Guid userId, 
            CancellationToken cancellationToken = default);
    }
}
