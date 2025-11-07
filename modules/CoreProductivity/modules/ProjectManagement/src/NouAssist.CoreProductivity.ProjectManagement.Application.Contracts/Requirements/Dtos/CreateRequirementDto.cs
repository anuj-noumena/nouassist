using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.CoreProductivity.ProjectManagement.Enums;

namespace NouAssist.CoreProductivity.ProjectManagement.Requirements.Dtos
{
    public class CreateRequirementDto
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }

        [Required]
        public ProjectRequirementType RequirementType { get; set; }
    }
}
