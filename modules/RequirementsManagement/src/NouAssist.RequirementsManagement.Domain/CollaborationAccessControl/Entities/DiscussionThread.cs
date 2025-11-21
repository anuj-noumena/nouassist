using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Exceptions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Entities
{
    /// <summary>
    /// Represents a thread of discussion about a specific resource.
    /// </summary>
    public class DiscussionThread : Entity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid DiscussionBoardId { get; private set; }
        public ThreadSubject Subject { get; private set; }
        public Guid CreatorId { get; private set; }
        public DateTime CreationTime { get; private set; }
        public bool IsClosed { get; private set; }
        public DateTime? ClosedTime { get; private set; }
        
        private readonly List<Comment> _comments;
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
        
        protected DiscussionThread()
        {
            _comments = new List<Comment>();
        }
        
        internal DiscussionThread(
            Guid id,
            Guid discussionBoardId,
            ThreadSubject subject,
            Guid creatorId,
            Guid? tenantId,
            DateTime creationTime) : base(id)
        {
            DiscussionBoardId = discussionBoardId;
            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            CreatorId = creatorId;
            TenantId = tenantId;
            CreationTime = creationTime;
            IsClosed = false;
            _comments = new List<Comment>();
        }
        
        /// <summary>
        /// Updates the thread subject.
        /// </summary>
        internal void UpdateSubject(ThreadSubject newSubject)
        {
            Subject = newSubject ?? throw new ArgumentNullException(nameof(newSubject));
        }
        
        /// <summary>
        /// Adds a comment to this thread.
        /// </summary>
        internal Comment AddComment(
            Guid commentId,
            Guid authorId,
            CommentContent content,
            DateTime creationTime)
        {
            if (IsClosed)
            {
                throw new CollaborationDomainException(
                    "Collaboration:ErrorMessage:InvalidOperation",
                    "Cannot add comments to a closed thread.");
            }
            
            var comment = new Comment(
                commentId,
                Id,
                authorId,
                content,
                TenantId,
                creationTime);
                
            _comments.Add(comment);
            return comment;
        }
        
        /// <summary>
        /// Closes this discussion thread.
        /// </summary>
        internal void Close(DateTime closedTime)
        {
            if (IsClosed)
            {
                return; // Already closed
            }
            
            IsClosed = true;
            ClosedTime = closedTime;
        }
        
        /// <summary>
        /// Reopens this discussion thread.
        /// </summary>
        internal void Reopen()
        {
            if (!IsClosed)
            {
                return; // Already open
            }
            
            IsClosed = false;
            ClosedTime = null;
        }
        
        /// <summary>
        /// Gets a comment by its ID.
        /// </summary>
        public Comment GetComment(Guid commentId)
        {
            return _comments.FirstOrDefault(c => c.Id == commentId);
        }
    }
}
