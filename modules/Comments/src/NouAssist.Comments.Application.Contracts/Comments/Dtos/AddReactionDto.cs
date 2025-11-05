using System.ComponentModel.DataAnnotations;

namespace NouAssist.Comments.Dtos
{
    public class AddReactionDto
    {
        [Required]
        [StringLength(10)]
        public string Emoji { get; set; }
    }
}
