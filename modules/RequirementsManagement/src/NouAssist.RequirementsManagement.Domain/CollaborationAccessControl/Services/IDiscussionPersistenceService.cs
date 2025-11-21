using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

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
