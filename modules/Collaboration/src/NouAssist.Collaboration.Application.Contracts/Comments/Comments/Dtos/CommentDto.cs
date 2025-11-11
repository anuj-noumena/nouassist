using System;
using System.Collections.Generic;
using NouAssist.Comments.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Comments.Dtos
{
    public class CommentDto : FullAuditedEntityDto<Guid>
    {
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public Guid? ParentCommentId { get; set; }
        public CommentableEntityType EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string Content { get; set; }
        public bool IsPinned { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditedAt { get; set; }
        public int ReactionsCount { get; set; }
        public int RepliesCount { get; set; }
        public List<Guid> MentionedUserIds { get; set; }
        public List<CommentReactionDto> Reactions { get; set; }

        // Computed properties
        public bool IsReply { get; set; }
        public bool HasReplies { get; set; }
        public bool HasReactions { get; set; }
        public bool HasMentions { get; set; }
    }
}
