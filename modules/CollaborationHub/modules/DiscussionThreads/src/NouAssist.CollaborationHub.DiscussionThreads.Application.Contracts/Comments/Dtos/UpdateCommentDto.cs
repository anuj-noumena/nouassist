using System.ComponentModel.DataAnnotations;

namespace NouAssist.CollaborationHub.DiscussionThreads.Dtos
{
    public class UpdateCommentDto
    {
        [Required]
        [StringLength(2000)]
        public string Content { get; set; }
    }
}
