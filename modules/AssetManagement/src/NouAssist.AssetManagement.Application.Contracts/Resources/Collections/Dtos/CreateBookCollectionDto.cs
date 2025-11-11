using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Resources.Collections.Dtos
{
    public class CreateBookCollectionDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public List<string> Tags { get; set; }
        public bool IsPublic { get; set; }
    }
}
