using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Resources.Books.Dtos
{
    public class UpdateBookDto
    {
        [Required]
        [MaxLength(500)]
        public string Title { get; set; }

        public List<string> Authors { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
    }
}
