using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.ProjectManagement.Constants;

namespace NouAssist.WorkManagement.ProjectManagement.Projects.Dtos
{
    public class UpdateProjectDto
    {
        [Required]
        [StringLength(ProjectDomainConstants.MaxTitleLength)]
        public string Title { get; set; }

        [StringLength(ProjectDomainConstants.MaxDescriptionLength)]
        public string Description { get; set; }

        [StringLength(ProjectDomainConstants.MaxDescriptionLength)]
        public string Vision { get; set; }

        [StringLength(ProjectDomainConstants.MaxDescriptionLength)]
        public string Purpose { get; set; }

        [StringLength(ProjectDomainConstants.MaxDescriptionLength)]
        public string ExpectedOutcome { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        [Range(0.0, 1.0)]
        public double Importance { get; set; }

        [Range(0.0, 1.0)]
        public double Urgency { get; set; }

        [Range(0, 100)]
        public double Progress { get; set; }

        public Guid? WorkflowSchemeId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? PrimaryTeamId { get; set; }
    }
}