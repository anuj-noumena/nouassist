using System.ComponentModel.DataAnnotations;

namespace NouAssist.Projects.Projects.Dtos
{
    public class UpdateProjectProgressDto
    {
        [Required]
        [Range(0, 100)]
        public double Progress { get; set; }
    }
}
