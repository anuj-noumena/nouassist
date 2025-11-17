using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.Tasks.Dtos;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos
{
    public class TaskInstanceDto : FullAuditedEntityDto<Guid>
    {
        // Core Identity
        public Guid TaskId { get; set; }
        public Guid WorkflowId { get; set; }
        public Guid WorkflowStatusId { get; set; }
        public Guid? ProjectId { get; set; }
        public int EntityVersion { get; set; }

        // Ownership
        public Guid OwnerId { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid? ReporterId { get; set; }
        public Guid? TeamId { get; set; }

        // GTD Organization
        public Guid? AreaId { get; set; }
        public Guid? ContextId { get; set; }

        // Work Tracking (from TaskWorkTracking value object)
        public double ProgressPercent { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int ActualMinutes { get; set; }
        public int? EstimatedMinutes { get; set; }
        public bool IsNextAction { get; set; }

        // Schedule (from TaskSchedule value object)
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedDueDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualDueDate { get; set; }

        // Priority (from ImportanceUrgencyMatrix value object)
        public double Importance { get; set; }
        public double Urgency { get; set; }
        public string EisenhowerQuadrant { get; set; }

        // Complexity and Cognitive Load
        public ComplexityEstimationDto ComplexityEstimation { get; set; }
        public CognitiveLoad? CognitiveLoad { get; set; }

        // Hierarchy (from TaskHierarchy value object)
        public Guid? ParentInstanceId { get; set; }
        public int HierarchyLevel { get; set; }

        // Collaboration (from TaskCollaboration value object)
        public int CommentsCount { get; set; }
        public int AttachmentsCount { get; set; }
        public List<Guid> Watchers { get; set; }

        // Blocking/Flagging
        public bool IsBlocked { get; set; }
        public string BlockedReason { get; set; }
        public DateTime? BlockedAt { get; set; }
        public bool IsFlagged { get; set; }
        public string FlaggedReason { get; set; }

        // Resolution
        public string Resolution { get; set; }
        public string ResolutionNote { get; set; }
        public bool IsResolved { get; set; }

        // Approval
        public bool RequiresApproval { get; set; }
        public Guid? ApprovalRequestId { get; set; }

        public DelegationInfoDto DelegationInfo { get; set; }

        // Computed properties
        public bool IsOverdue { get; set; }
        public bool IsCompleted { get; set; }
        public bool HasStarted { get; set; }
        public bool IsDelegated { get; set; }
        public bool HasBlockingDependencies { get; set; }
        public TimeSpan? TimeRemaining { get; set; }
        
        // GTD Computed properties
        public bool IsInArea { get; set; }
        public bool IsInProject { get; set; }
        public bool IsInContext { get; set; }
        public bool IsOrganized { get; set; }
        public bool IsAreaTask { get; set; }
        public bool IsProjectTask { get; set; }
    }

    public class DelegationInfoDto
    {
        public Guid DelegatedBy { get; set; }
        public Guid DelegatedTo { get; set; }
        public string Status { get; set; }
        public DateTime DelegatedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public string Note { get; set; }
        public string ResponseNote { get; set; }
    }
}