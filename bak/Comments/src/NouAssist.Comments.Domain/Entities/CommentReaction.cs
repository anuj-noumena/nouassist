using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Comments.Entities
{
    public class CommentReaction : Entity<Guid>
    {
        public Guid CommentId { get; private set; }
        public Guid UserId { get; private set; }
        public string Emoji { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected CommentReaction() { }

        public CommentReaction(Guid id, Guid commentId, Guid userId, string emoji)
            : base(id)
        {
            CommentId = commentId;
            UserId = userId;
            Emoji = Check.NotNullOrWhiteSpace(emoji, nameof(emoji));
            CreatedAt = DateTime.UtcNow;
        }
    }
}
