using System;
using System.Collections.Generic;
using NouAssist.Approval.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Approval.Workflows.Dtos
{
    public class ApprovalWorkflowDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ApprovalWorkflowType WorkflowType { get; set; }
        public ApprovalStrategy Strategy { get; set; }
        public bool IsActive { get; set; }
        public bool AllowReassign { get; set; }
        public bool AllowDelegate { get; set; }
        public int? TimeoutDays { get; set; }
        public string TimeoutAction { get; set; }
        public List<ApprovalStepDto> Steps { get; set; }
    }

    public class ApprovalStepDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderIndex { get; set; }
        public ApprovalRequirement Requirement { get; set; }
        public int? RequiredApprovals { get; set; }
        public bool AllowSelfApproval { get; set; }
        public int? TimeoutHours { get; set; }
        public List<Guid> ApproverUserIds { get; set; }
        public List<Guid> ApproverRoleIds { get; set; }
        public List<Guid> ApproverTeamIds { get; set; }
    }

    public class CreateApprovalWorkflowDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ApprovalWorkflowType WorkflowType { get; set; }
        public ApprovalStrategy Strategy { get; set; }
    }

    public class UpdateApprovalWorkflowDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TimeoutDays { get; set; }
        public string TimeoutAction { get; set; }
        public bool AllowReassign { get; set; }
        public bool AllowDelegate { get; set; }
    }

    public class CreateApprovalStepDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderIndex { get; set; }
        public ApprovalRequirement Requirement { get; set; }
        public int? RequiredApprovals { get; set; }
        public bool AllowSelfApproval { get; set; }
        public int? TimeoutHours { get; set; }
        public List<Guid> ApproverUserIds { get; set; }
        public List<Guid> ApproverRoleIds { get; set; }
        public List<Guid> ApproverTeamIds { get; set; }
    }

    public class GetApprovalWorkflowListDto : PagedAndSortedResultRequestDto
    {
        public ApprovalWorkflowType? WorkflowType { get; set; }
        public bool? IsActive { get; set; }
        public string Filter { get; set; }
    }
}
