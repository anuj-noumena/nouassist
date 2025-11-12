using System;
using System.Collections.Generic;
using NouAssist.SharedKernel.Enums;
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
        public Guid? ContextId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? TenantId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ExpectedOutcome { get; set; }

        public ProjectStatus Status { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? ArchivedAt { get; set; }

        public double Importance { get; set; }
        public double Urgency { get; set; }
        public EisenhowerQuadrant EisenhowerQuadrant { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public double Progress { get; set; }
        public DateTime? LastReviewedAt { get; set; }

        public Guid? PrimaryTeamId { get; set; }
        public List<Guid> CollaboratingTeamIds { get; set; }
        public List<Guid> SuccessCriterionIds { get; set; }

        public bool RequiresApproval { get; set; }
        public Guid? ApprovalWorkflowId { get; set; }
        public Guid? CurrentApprovalRequestId { get; set; }

        // Computed Properties
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsOverdue { get; set; }
        public int? DaysRemaining { get; set; }
        public ProjectHealth Health { get; set; }
        public double SuccessCriteriaCompletionRate { get; set; }
    }
}
