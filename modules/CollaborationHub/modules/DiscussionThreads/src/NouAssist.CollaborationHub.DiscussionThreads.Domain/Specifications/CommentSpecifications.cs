using System;
using System.Linq.Expressions;
using NouAssist.CollaborationHub.DiscussionThreads.Aggregates;
using NouAssist.CollaborationHub.DiscussionThreads.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.CollaborationHub.DiscussionThreads.Specifications
{
    public class CommentsByEntitySpecification : Specification<Comment>
    {
        private readonly CommentableEntityType _entityType;
        private readonly Guid _entityId;

        public CommentsByEntitySpecification(CommentableEntityType entityType, Guid entityId)
        {
            _entityType = entityType;
            _entityId = entityId;
        }

        public override Expression<Func<Comment, bool>> ToExpression() =>
            c => c.EntityType == _entityType && c.EntityId == _entityId && !c.IsDeleted;
    }

    public class TopLevelCommentsSpecification : Specification<Comment>
    {
        public override Expression<Func<Comment, bool>> ToExpression() =>
            c => c.ParentCommentId == null && !c.IsDeleted;
    }
}
