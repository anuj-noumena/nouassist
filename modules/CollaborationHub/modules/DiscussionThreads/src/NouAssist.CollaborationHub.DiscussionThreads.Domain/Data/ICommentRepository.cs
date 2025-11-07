using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.CollaborationHub.DiscussionThreads.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CollaborationHub.DiscussionThreads.Data
{
    public interface ICommentRepository : IRepository<Aggregates.Comment, Guid>
    {
        Task<List<Aggregates.Comment>> GetCommentsByEntityAsync(
            CommentableEntityType entityType,
            Guid entityId,
            CancellationToken cancellationToken = default);

        Task<List<Aggregates.Comment>> GetRepliesAsync(
            Guid parentCommentId,
            CancellationToken cancellationToken = default);
    }
}
