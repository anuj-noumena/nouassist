using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Specifications
{
    /// <summary>
    /// Base specification for discussion threads.
    /// </summary>
    public abstract class DiscussionThreadSpecification : Specification<DiscussionThread>
    {
    }

    /// <summary>
    /// Specification for open discussion threads.
    /// </summary>
    public class OpenThreadSpecification : DiscussionThreadSpecification
    {
        public override Expression<Func<DiscussionThread, bool>> ToExpression()
        {
            return thread => !thread.IsClosed;
        }
    }

    /// <summary>
    /// Specification for discussion threads created by a specific user.
    /// </summary>
    public class ThreadsByCreatorSpecification : DiscussionThreadSpecification
    {
        private readonly Guid _creatorId;

        public ThreadsByCreatorSpecification(Guid creatorId)
        {
            _creatorId = creatorId;
        }

        public override Expression<Func<DiscussionThread, bool>> ToExpression()
        {
            return thread => thread.CreatorId == _creatorId;
        }
    }

    /// <summary>
    /// Specification for discussion threads created after a specific date.
    /// </summary>
    public class RecentThreadsSpecification : DiscussionThreadSpecification
    {
        private readonly DateTime _sinceDate;

        public RecentThreadsSpecification(DateTime sinceDate)
        {
            _sinceDate = sinceDate;
        }

        public override Expression<Func<DiscussionThread, bool>> ToExpression()
        {
            return thread => thread.CreationTime >= _sinceDate;
        }
    }

    /// <summary>
    /// Specification for discussion threads containing comments from a specific user.
    /// </summary>
    public class ThreadsWithCommentsFromUserSpecification : DiscussionThreadSpecification
    {
        private readonly Guid _userId;

        public ThreadsWithCommentsFromUserSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<DiscussionThread, bool>> ToExpression()
        {
            return thread => thread.Comments.Any(c => c.AuthorId == _userId);
        }
    }
}
