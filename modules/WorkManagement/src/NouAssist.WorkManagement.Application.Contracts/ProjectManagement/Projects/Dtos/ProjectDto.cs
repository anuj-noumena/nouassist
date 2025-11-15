using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.ProjectManagement.Projects.Dtos
{
    public class ProjectDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public Guid? ParentProjectId { get; set; }
        public Guid? GoalId { get; set; }
        public Guid? WorkflowSchemeId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? TenantId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Vision { get; set; }
        public string Purpose { get; set; }
        public string ExpectedOutcome { get; set; }

        public ProjectStatus Status { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? ArchivedAt { get; set; }

        // Priority (from value object)
        public double Importance { get; set; }
        public double Urgency { get; set; }

        // Timeline (from value object)
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedDueDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualCompletedDate { get; set; }

        public double Progress { get; set; }
        public DateTime? LastReviewedAt { get; set; }

        public Guid? PrimaryTeamId { get; set; }
        public List<Guid> CollaboratingTeamIds { get; set; }

        public Guid? ApprovalRequestId { get; set; }

        // Computed Properties
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsOverdue { get; set; }
        public ProjectHealth Health { get; set; }
        public int SuccessCriteriaMet { get; set; }
        public double SuccessCriteriaCompletionRate { get; set; }
        public bool HasSuccessCriteria { get; set; }
    }
}