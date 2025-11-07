using System.ComponentModel.DataAnnotations;

namespace NouAssist.CollaborationHub.Resources.Books.Dtos
{
    public class RateBookDto
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string Review { get; set; }
    }
}
