using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.ProjectManagement.Enums;

namespace NouAssist.WorkManagement.ProjectManagement.Dependencies.Dtos
{
    public class CreateDependencyDto
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public Guid DependsOnProjectId { get; set; }

        [Required]
        public ProjectDependencyType DependencyType { get; set; }

        [Range(0, int.MaxValue)]
        public int LagDays { get; set; } = 0;

        public bool IsBlocking { get; set; } = true;

        [StringLength(2000)]
        public string Description { get; set; }

        public DateTime? ExpectedResolutionDate { get; set; }
    }
}
