using System.ComponentModel.DataAnnotations;

namespace NouAssist.CoreProductivity.ProjectManagement.Requirements.Dtos
{
    public class UpdateRequirementDto
    {
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }
    }
}
