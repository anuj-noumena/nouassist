using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.ProjectManagement.Sprints.Dtos
{
    public class CreateSprintDto
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Goal { get; set; }

        [StringLength(500)]
        public string ReleaseTheme { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(0, ProjectDomainConstants.MaxStoryPoints)]
        public int VelocityEstimate { get; set; }
    }
}
