namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Services
{
    /// <summary>
    /// Interface for discussion persistence service.
    /// </summary>
    public interface IDiscussionPersistenceService
    {
        Task<DiscussionBoard> GetOrCreateDiscussionBoardAsync(
            TargetResourceType resourceType,
            Guid resourceId,
            Guid currentUserId);
            
        Task<DiscussionThread> CreateThreadAsync(
            Guid discussionBoardId,
            string subject,
            string initialComment,
            Guid currentUserId);
            
        Task<Comment> AddCommentAsync(
            Guid discussionBoardId,
            Guid threadId,
            string content,
            Guid currentUserId);
    }
}
