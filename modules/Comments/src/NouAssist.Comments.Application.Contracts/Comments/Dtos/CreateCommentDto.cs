using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.Comments.Enums;

namespace NouAssist.Comments.Dtos
{
    public class CreateCommentDto
    {
        [Required]
        public CommentableEntityType EntityType { get; set; }

        [Required]
        public Guid EntityId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }

        public Guid? ParentCommentId { get; set; }

        public List<Guid> MentionedUserIds { get; set; } = new();
    }
}
