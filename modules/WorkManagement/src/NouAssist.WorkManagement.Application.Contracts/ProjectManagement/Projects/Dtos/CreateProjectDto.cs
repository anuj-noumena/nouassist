using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.ProjectManagement.Projects.Dtos
{
    public class CreateProjectDto
    {
        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        [StringLength(ProjectDomainConstants.MaxTitleLength)]
        public string Title { get; set; }

        [StringLength(ProjectDomainConstants.MaxDescriptionLength)]
        public string Description { get; set; }

        [StringLength(ProjectDomainConstants.MaxDescriptionLength)]
        public string ExpectedOutcome { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        [Range(0.0, 1.0)]
        public double Importance { get; set; } = 0.5;

        [Range(0.0, 1.0)]
        public double Urgency { get; set; } = 0.5;

        public Guid? ParentProjectId { get; set; }
        public Guid? GoalId { get; set; }
        public Guid? ContextId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? PrimaryTeamId { get; set; }
        public bool RequiresApproval { get; set; }
    }
}
