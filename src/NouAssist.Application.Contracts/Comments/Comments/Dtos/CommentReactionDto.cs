using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Comments.Dtos
{
    public class CommentReactionDto : EntityDto<Guid>
    {
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Emoji { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
