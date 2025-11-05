using System;
using NouAssist.Comments.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Comments.Dtos
{
    public class GetCommentListDto : PagedAndSortedResultRequestDto
    {
        public CommentableEntityType? EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public Guid? AuthorId { get; set; }
        public bool? IsPinned { get; set; }
        public bool? OnlyTopLevel { get; set; }
    }
}
