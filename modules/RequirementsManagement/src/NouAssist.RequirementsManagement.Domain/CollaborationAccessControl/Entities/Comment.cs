using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Entities
{
    /// <summary>
    /// Represents a comment in a discussion thread.
    /// </summary>
    public class Comment : Entity<Guid>, IMultiTenant, IHasCreationTime
    {
        public Guid? TenantId { get; protected set; }
        public Guid ThreadId { get; private set; }
        public Guid AuthorId { get; private set; }
        public CommentContent Content { get; private set; }
        public DateTime CreationTime { get; private set; }
        public bool IsEdited { get; private set; }
        public DateTime? LastEditTime { get; private set; }
        
        protected Comment() { }
        
        internal Comment(
            Guid id,
            Guid threadId,
            Guid authorId,
            CommentContent content,
            Guid? tenantId,
            DateTime creationTime) : base(id)
        {
            ThreadId = threadId;
            AuthorId = authorId;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            TenantId = tenantId;
            CreationTime = creationTime;
            IsEdited = false;
        }
        
        /// <summary>
        /// Updates the comment content.
        /// </summary>
        internal void UpdateContent(CommentContent newContent, DateTime editTime)
        {
            if (newContent == null)
            {
                throw new ArgumentNullException(nameof(newContent));
            }
            
            Content = newContent;
            IsEdited = true;
            LastEditTime = editTime;
        }
        
        /// <summary>
        /// Gets the list of user IDs mentioned in this comment.
        /// </summary>
        public List<Guid> GetMentionedUserIds()
        {
            return Content.MentionedUserIds;
        }
    }
}
