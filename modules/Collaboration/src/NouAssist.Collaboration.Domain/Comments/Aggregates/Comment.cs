using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Collaboration.Comments.Enums;
using NouAssist.Collaboration.Comments.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Collaboration.Comments.Aggregates
{
    public class Comment : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid AuthorId { get; private set; }
        public Guid? ParentCommentId { get; private set; }
        public CommentableEntityType EntityType { get; private set; }
        public Guid EntityId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Content { get; private set; }
        public bool IsPinned { get; private set; }
        public bool IsEdited { get; private set; }
        public DateTime? EditedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public int ReactionsCount { get; private set; }
        public int RepliesCount { get; private set; }

        private readonly List<Guid> _mentionedUserIds = new();
        public IReadOnlyCollection<Guid> MentionedUserIds => _mentionedUserIds.AsReadOnly();

        private readonly List<Entities.CommentReaction> _reactions = new();
        public IReadOnlyCollection<Entities.CommentReaction> Reactions => _reactions.AsReadOnly();

        protected Comment() { }

        public Comment(
            Guid id,
            Guid authorId,
            CommentableEntityType entityType,
            Guid entityId,
            string content,
            Guid? tenantId = null,
            Guid? parentCommentId = null,
            List<Guid> mentionedUserIds = null)
            : base(id)
        {
            AuthorId = authorId;
            EntityType = entityType;
            EntityId = entityId;
            SetContent(content);
            TenantId = tenantId;
            ParentCommentId = parentCommentId;

            if (mentionedUserIds != null && mentionedUserIds.Any())
            {
                if (mentionedUserIds.Count > 10) // DomainConstants.maxMentionsPerComment
                {
                    throw new BusinessException("Comments:TooManyMentions")
                        .WithData("maxMentions", 10);
                }

                _mentionedUserIds.AddRange(mentionedUserIds);
            }

            AddDistributedEvent(new CommentCreatedEto
            {
                CommentId = Id,
                AuthorId = AuthorId,
                EntityType = EntityType.ToString(),
                EntityId = EntityId,
                TenantId = TenantId,
                IsReply = ParentCommentId.HasValue,
                CreatedAt = DateTime.UtcNow
            });
        }

        public void SetContent(string content)
        {
            Content = Check.NotNullOrWhiteSpace(content, nameof(content), maxLength: 2000);
        }

        public void Edit(string newContent)
        {
            if (IsDeleted)
            {
                throw new BusinessException("Comments:CannotEditDeleted");
            }

            SetContent(newContent);
            IsEdited = true;
            EditedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            if (IsDeleted)
            {
                return;
            }

            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void Pin()
        {
            IsPinned = true;
        }

        public void Unpin()
        {
            IsPinned = false;
        }

        public void AddReaction(Guid userId, string emoji)
        {
            var existingReaction = _reactions.FirstOrDefault(r => r.UserId == userId && r.Emoji == emoji);
            if (existingReaction != null)
            {
                return;
            }

            var reaction = new Entities.CommentReaction(Guid.NewGuid(), Id, userId, emoji);
            _reactions.Add(reaction);
            ReactionsCount++;
        }

        public void RemoveReaction(Guid userId, string emoji)
        {
            var reaction = _reactions.FirstOrDefault(r => r.UserId == userId && r.Emoji == emoji);
            if (reaction == null)
            {
                return;
            }

            _reactions.Remove(reaction);
            ReactionsCount = Math.Max(0, ReactionsCount - 1);
        }

        public void IncrementRepliesCount()
        {
            RepliesCount++;
        }

        public bool IsReply => ParentCommentId.HasValue;
        public bool HasReplies => RepliesCount > 0;
        public bool HasReactions => ReactionsCount > 0;
        public bool HasMentions => _mentionedUserIds.Any();
    }
}
