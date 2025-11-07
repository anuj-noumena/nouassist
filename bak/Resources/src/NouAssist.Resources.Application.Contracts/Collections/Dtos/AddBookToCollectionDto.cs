using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Resources.Collections.Dtos
{
    public class AddBookToCollectionDto
    {
        [Required]
        public Guid BookId { get; set; }
    }
}
