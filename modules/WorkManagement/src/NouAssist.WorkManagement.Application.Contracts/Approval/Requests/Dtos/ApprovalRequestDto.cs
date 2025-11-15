using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.Approval.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.Approval.Requests.Dtos
{
    public class ApprovalRequestDto : FullAuditedEntityDto<Guid>
    {
        public Guid WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public Guid RequestedBy { get; set; }
        public string RequestedByName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ApprovalStatus Status { get; set; }
        public Guid? CurrentStepId { get; set; }
        public string CurrentStepName { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public Guid? CancelledBy { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? SlaDeadline { get; set; }
        public int Priority { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsSlaBreached { get; set; }
        public TimeSpan? TimeRemaining { get; set; }
        public Dictionary<string, object> CustomFields { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public List<Guid> CompletedStepIds { get; set; }
        public List<ApprovalDecisionDto> Decisions { get; set; }
    }

    public class ApprovalDecisionDto : EntityDto<Guid>
    {
        public Guid RequestId { get; set; }
        public Guid StepId { get; set; }
        public string StepName { get; set; }
        public Guid ApproverId { get; set; }
        public string ApproverName { get; set; }
        public ApprovalAction Action { get; set; }
        public string Comments { get; set; }
        public string Signature { get; set; }
        public DateTime DecidedAt { get; set; }
        public bool IsDelegated { get; set; }
        public Guid? DelegatedFrom { get; set; }
        public Dictionary<string, object> CustomFieldValues { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }

    public class CreateApprovalRequestDto
    {
        public Guid WorkflowId { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? SlaDeadline { get; set; }
        public Dictionary<string, object> CustomFields { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }

    public class MakeApprovalDecisionDto
    {
        public ApprovalAction Action { get; set; }
        public string Comments { get; set; }
        public string Signature { get; set; }
        public Dictionary<string, object> CustomFieldValues { get; set; }
    }

    public class GetApprovalRequestListDto : PagedAndSortedResultRequestDto
    {
        public ApprovalStatus? Status { get; set; }
        public Guid? WorkflowId { get; set; }
        public string EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public Guid? RequestedBy { get; set; }
        public bool? IsOverdue { get; set; }
        public bool? IsSlaBreached { get; set; }
        public string Filter { get; set; }
    }

    public class UpdateApprovalRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? SlaDeadline { get; set; }
    }

    public class SetCustomFieldDto
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
