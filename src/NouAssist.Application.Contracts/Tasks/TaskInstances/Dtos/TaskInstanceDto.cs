using System;
using System.Collections.Generic;
using NouAssist.Tasks.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Tasks.TaskInstances.Dtos
{
    public class TaskInstanceDto : FullAuditedEntityDto<Guid>
    {
        public Guid TaskId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid WorkflowStatusId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid? ReporterId { get; set; }
        public Guid? TeamId { get; set; }

        public string TaskStatus { get; set; }
        public double ProgressPercent { get; set; }

        public double Importance { get; set; }
        public double Urgency { get; set; }
        public string EisenhowerQuadrant { get; set; }

        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedDueDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualDueDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int ActualMinutes { get; set; }
        public DateTime? LastTrackedAt { get; set; }

        public int OrderInParent { get; set; }
        public Guid? ParentInstanceId { get; set; }
        public int SubtasksCount { get; set; }
        public int SubtasksDoneCount { get; set; }

        public int CommentsCount { get; set; }
        public int AttachmentsCount { get; set; }
        public List<Guid> Watchers { get; set; }
        public bool IsDiscussionLocked { get; set; }

        public bool IsNextAction { get; set; }
        public bool IsBlocked { get; set; }
        public string BlockedReason { get; set; }
        public DateTime? BlockedAt { get; set; }
        public bool IsFlagged { get; set; }
        public string FlaggedReason { get; set; }

        public string Resolution { get; set; }
        public string ResolutionNote { get; set; }

        public bool RequiresApproval { get; set; }
        public Guid? ApprovalRequestId { get; set; }

        public DelegationInfoDto DelegationInfo { get; set; }

        public int Version { get; set; }

        // Computed properties
        public bool IsOverdue { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCancelled { get; set; }
        public bool HasStarted { get; set; }
        public bool IsDelegated { get; set; }
        public double SubtaskCompletionRate { get; set; }
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
