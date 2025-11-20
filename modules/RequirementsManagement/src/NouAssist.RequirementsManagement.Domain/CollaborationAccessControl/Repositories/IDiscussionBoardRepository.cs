namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories
{
    /// <summary>
    /// Repository for DiscussionBoard aggregate.
    /// </summary>
    public interface IDiscussionBoardRepository : IRepository<DiscussionBoard, Guid>
    {
        /// <summary>
        /// Gets a discussion board by resource.
        /// </summary>
        Task<DiscussionBoard> GetByResourceAsync(
            TargetResourceType resourceType,
            Guid resourceId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets discussion boards containing threads created by a specific user.
        /// </summary>
        Task<List<DiscussionBoard>> GetByThreadCreatorAsync(
            Guid creatorId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets discussion boards containing active threads.
        /// </summary>
        Task<List<DiscussionBoard>> GetWithActiveThreadsAsync(
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets discussion boards by resource type.
        /// </summary>
        Task<List<DiscussionBoard>> GetByResourceTypeAsync(
            TargetResourceType resourceType,
            CancellationToken cancellationToken = default);
    }
}
