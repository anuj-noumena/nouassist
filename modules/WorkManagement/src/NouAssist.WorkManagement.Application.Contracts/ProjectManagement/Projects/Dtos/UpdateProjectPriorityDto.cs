using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.ProjectManagement.Projects.Dtos
{
    public class UpdateProjectPriorityDto
    {
        [Required]
        [Range(0.0, 1.0)]
        public double Importance { get; set; }

        [Required]
        [Range(0.0, 1.0)]
        public double Urgency { get; set; }
    }
}
