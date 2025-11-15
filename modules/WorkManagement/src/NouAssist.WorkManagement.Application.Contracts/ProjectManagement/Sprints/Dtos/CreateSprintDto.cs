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

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int VelocityEstimate { get; set; } = 0;
    }
}