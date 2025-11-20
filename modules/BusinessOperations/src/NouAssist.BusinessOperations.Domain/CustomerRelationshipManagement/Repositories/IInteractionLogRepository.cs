namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    /// <summary>
    /// Repository interface for InteractionLog aggregate
    /// </summary>
    public interface IInteractionLogRepository : IRepository<InteractionLog, Guid>
    {
        /// <summary>
        /// Gets interaction logs for the specified entity
        /// </summary>
        Task<List<InteractionLog>> GetForEntityAsync(
            Guid entityId, 
            string entityType, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets interaction logs with communications of the specified type
        /// </summary>
        Task<List<InteractionLog>> GetByInteractionTypeAsync(
            InteractionType type, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets interaction logs with pending follow-up tasks
        /// </summary>
        Task<List<InteractionLog>> GetWithPendingFollowUpsAsync(
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
    }
}
