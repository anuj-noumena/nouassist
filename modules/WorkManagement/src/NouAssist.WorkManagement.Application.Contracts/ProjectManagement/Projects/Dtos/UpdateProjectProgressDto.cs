using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.ProjectManagement.Projects.Dtos
{
    public class UpdateProjectProgressDto
    {
        [Required]
        [Range(0, 100)]
        public double Progress { get; set; }
    }
}
