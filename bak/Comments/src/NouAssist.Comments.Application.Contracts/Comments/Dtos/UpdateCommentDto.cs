using System.ComponentModel.DataAnnotations;

namespace NouAssist.Comments.Dtos
{
    public class UpdateCommentDto
    {
        [Required]
        [StringLength(2000)]
        public string Content { get; set; }
    }
}
