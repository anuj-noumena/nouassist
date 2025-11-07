using System.ComponentModel.DataAnnotations;

namespace NouAssist.CollaborationHub.DiscussionThreads.Dtos
{
    public class AddReactionDto
    {
        [Required]
        [StringLength(10)]
        public string Emoji { get; set; }
    }
}
