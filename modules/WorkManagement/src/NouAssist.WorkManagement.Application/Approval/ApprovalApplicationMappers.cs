using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Entities;
using NouAssist.WorkManagement.Approval.Requests.Dtos;
using NouAssist.WorkManagement.Approval.Workflows.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.WorkManagement.Approval
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ApprovalWorkflowToApprovalWorkflowDtoMapper : MapperBase<ApprovalWorkflow, ApprovalWorkflowDto>
    {
        [MapperIgnoreTarget(nameof(ApprovalWorkflowDto.CustomFields))]
        [MapperIgnoreTarget(nameof(ApprovalWorkflowDto.TotalSteps))]
        public override partial ApprovalWorkflowDto Map(ApprovalWorkflow source);

        public override partial void Map(ApprovalWorkflow source, ApprovalWorkflowDto destination);

        public override void AfterMap(ApprovalWorkflow source, ApprovalWorkflowDto destination)
        {
            destination.CustomFields = source.CustomFields.GetValues();
            destination.TotalSteps = source.GetTotalSteps();
            destination.CanBeActivated = source.CanBeActivated;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ApprovalStepToApprovalStepDtoMapper : MapperBase<ApprovalStep, ApprovalStepDto>
    {
        [MapperIgnoreTarget(nameof(ApprovalStepDto.CustomFields))]
        public override partial ApprovalStepDto Map(ApprovalStep source);

        public override partial void Map(ApprovalStep source, ApprovalStepDto destination);

        public override void AfterMap(ApprovalStep source, ApprovalStepDto destination)
        {
            destination.ApproverUserIds = (List<Guid>)source.ApproverUserIds;
            destination.ApproverRoleIds = (List<Guid>)source.ApproverRoleIds;
            destination.ApproverTeamIds = (List<Guid>)source.ApproverTeamIds;
            destination.CustomFields = source.CustomFields.GetValues();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ApprovalRequestToApprovalRequestDtoMapper : MapperBase<ApprovalRequest, ApprovalRequestDto>
    {
        [MapperIgnoreTarget(nameof(ApprovalRequestDto.CustomFields))]
        [MapperIgnoreTarget(nameof(ApprovalRequestDto.Metadata))]
        [MapperIgnoreTarget(nameof(ApprovalRequestDto.CompletedStepIds))]
        [MapperIgnoreTarget(nameof(ApprovalRequestDto.WorkflowName))]
        [MapperIgnoreTarget(nameof(ApprovalRequestDto.RequestedByName))]
        [MapperIgnoreTarget(nameof(ApprovalRequestDto.CurrentStepName))]
        public override partial ApprovalRequestDto Map(ApprovalRequest source);

        public override partial void Map(ApprovalRequest source, ApprovalRequestDto destination);

        public override void AfterMap(ApprovalRequest source, ApprovalRequestDto destination)
        {
            destination.IsOverdue = source.IsOverdue;
            destination.IsSlaBreached = source.IsSlaBreached;
            destination.TimeRemaining = source.TimeRemaining;
            destination.CustomFields = source.CustomFields.GetValues();
            destination.Metadata = source.Metadata.GetValues();
            destination.CompletedStepIds = source.CompletedStepIds.ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ApprovalDecisionToApprovalDecisionDtoMapper : MapperBase<ApprovalDecision, ApprovalDecisionDto>
    {
        [MapperIgnoreTarget(nameof(ApprovalDecisionDto.StepName))]
        [MapperIgnoreTarget(nameof(ApprovalDecisionDto.ApproverName))]
        public override partial ApprovalDecisionDto Map(ApprovalDecision source);

        public override partial void Map(ApprovalDecision source, ApprovalDecisionDto destination);
    }
}
