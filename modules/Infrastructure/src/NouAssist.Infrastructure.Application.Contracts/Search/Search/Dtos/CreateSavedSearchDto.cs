using System.ComponentModel.DataAnnotations;

namespace NouAssist.Infrastructure.Search.Search.Dtos
{
    public class CreateSavedSearchDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Query { get; set; }

        public string Filters { get; set; }
    }
}
