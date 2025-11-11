using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.Resources.Enums;

namespace NouAssist.Resources.Books.Dtos
{
    public class CreateBookDto
    {
        [Required]
        public Guid ResourceId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Title { get; set; }

        public List<string> Authors { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public BookFormat Format { get; set; } = BookFormat.Pdf;
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
    }
}
