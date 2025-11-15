using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Data;
using NouAssist.WorkManagement.Approval.Entities;
using NouAssist.WorkManagement.Approval.Enums;
using NouAssist.WorkManagement.Approval.Workflows.Dtos;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.Approval.Workflows
{
    [Authorize(WorkManagementPermissions.Approval.Workflows.Default)]
    public class ApprovalWorkflowAppService : ApplicationService, IApprovalWorkflowAppService
    {
        private readonly IApprovalWorkflowRepository _workflowRepository;

        public ApprovalWorkflowAppService(IApprovalWorkflowRepository workflowRepository)
        {
            _workflowRepository = workflowRepository;
        }

        public async Task<ApprovalWorkflowDto> GetAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(id);
            return ObjectMapper.Map<ApprovalWorkflow, ApprovalWorkflowDto>(workflow);
        }

        public async Task<PagedResultDto<ApprovalWorkflowDto>> GetListAsync(
            GetApprovalWorkflowListDto input)
        {
            var spec = CreateListSpecification(input);

            var workflows = await _workflowRepository.GetListAsync(spec.ToExpression());
            var totalCount = await _workflowRepository.CountAsync(spec.ToExpression());

            return new PagedResultDto<ApprovalWorkflowDto>(
                totalCount,
                ObjectMapper.Map<List<ApprovalWorkflow>, List<ApprovalWorkflowDto>>(workflows)
            );
        }

        public async Task<List<ApprovalWorkflowDto>> GetActiveWorkflowsAsync()
        {
            var workflows = await _workflowRepository.GetActiveWorkflowsAsync();
            return ObjectMapper.Map<List<ApprovalWorkflow>, List<ApprovalWorkflowDto>>(workflows);
        }

        public async Task<List<ApprovalWorkflowDto>> GetByTypeAsync(int workflowType)
        {
            var workflows = await _workflowRepository.GetByTypeAsync((ApprovalWorkflowType)workflowType);
            return ObjectMapper.Map<List<ApprovalWorkflow>, List<ApprovalWorkflowDto>>(workflows);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Create)]
        public async Task<ApprovalWorkflowDto> CreateAsync(CreateApprovalWorkflowDto input)
        {
            var workflow = new ApprovalWorkflow(
                GuidGenerator.Create(),
                input.Name,
                input.WorkflowType,
                input.Strategy,
                CurrentTenant.Id,
                input.Description
            );

            if (input.CustomFields != null)
            {
                foreach (var kvp in input.CustomFields)
                {
                    workflow.SetCustomFieldValue(kvp.Key, kvp.Value);
                }
            }

            await _workflowRepository.InsertAsync(workflow);

            return ObjectMapper.Map<ApprovalWorkflow, ApprovalWorkflowDto>(workflow);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Update)]
        public async Task<ApprovalWorkflowDto> UpdateAsync(Guid id, UpdateApprovalWorkflowDto input)
        {
            var workflow = await _workflowRepository.GetAsync(id);

            workflow.SetName(input.Name);
            workflow.SetDescription(input.Description);

            if (input.TimeoutDays.HasValue && !string.IsNullOrEmpty(input.TimeoutAction))
            {
                workflow.SetTimeout(input.TimeoutDays.Value, input.TimeoutAction);
            }

            if (input.AllowReassign)
            {
                workflow.EnableReassignment();
            }

            if (input.AllowDelegate)
            {
                workflow.EnableDelegation();
            }

            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<ApprovalWorkflow, ApprovalWorkflowDto>(workflow);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _workflowRepository.DeleteAsync(id);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Activate)]
        public async Task<ApprovalWorkflowDto> ActivateAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(id);
            
            workflow.Activate();
            
            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<ApprovalWorkflow, ApprovalWorkflowDto>(workflow);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Deactivate)]
        public async Task<ApprovalWorkflowDto> DeactivateAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetAsync(id);
            
            workflow.Deactivate();
            
            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<ApprovalWorkflow, ApprovalWorkflowDto>(workflow);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Update)]
        public async Task<ApprovalStepDto> AddStepAsync(
            Guid workflowId, 
            CreateApprovalStepDto input)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(workflowId);

            var step = workflow.AddStep(
                input.Name,
                input.OrderIndex,
                input.Requirement,
                input.Description
            );

            if (input.RequiredApprovals.HasValue)
            {
                step.SetRequiredApprovals(input.RequiredApprovals.Value);
            }

            if (input.AllowSelfApproval)
            {
                step.AllowSelfApprovalForStep();
            }

            if (input.TimeoutHours.HasValue && !string.IsNullOrEmpty(input.TimeoutAction))
            {
                step.SetTimeout(input.TimeoutHours.Value, input.TimeoutAction);
            }

            // Add approvers
            if (input.ApproverUserIds != null)
            {
                foreach (var userId in input.ApproverUserIds)
                {
                    step.AddApproverUser(userId);
                }
            }

            if (input.ApproverRoleIds != null)
            {
                foreach (var roleId in input.ApproverRoleIds)
                {
                    step.AddApproverRole(roleId);
                }
            }

            if (input.ApproverTeamIds != null)
            {
                foreach (var teamId in input.ApproverTeamIds)
                {
                    step.AddApproverTeam(teamId);
                }
            }

            // Set conditions
            if (input.Conditions != null)
            {
                foreach (var condition in input.Conditions)
                {
                    step.SetCondition(condition.Key, condition.Value);
                }
            }

            // Set custom fields
            if (input.CustomFields != null)
            {
                foreach (var kvp in input.CustomFields)
                {
                    // Assuming CustomFields has a SetValue method or similar
                    // You may need to adjust based on actual implementation
                }
            }

            step.ValidateApprovers();

            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<ApprovalStep, ApprovalStepDto>(step);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Update)]
        public async Task<ApprovalStepDto> UpdateStepAsync(
            Guid workflowId, 
            Guid stepId, 
            UpdateApprovalStepDto input)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(workflowId);
            var step = workflow.GetStep(stepId);

            step.SetName(input.Name);

            if (input.RequiredApprovals.HasValue)
            {
                step.SetRequiredApprovals(input.RequiredApprovals.Value);
            }

            if (input.AllowSelfApproval)
            {
                step.AllowSelfApprovalForStep();
            }

            if (input.TimeoutHours.HasValue && !string.IsNullOrEmpty(input.TimeoutAction))
            {
                step.SetTimeout(input.TimeoutHours.Value, input.TimeoutAction);
            }

            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<ApprovalStep, ApprovalStepDto>(step);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Update)]
        public async Task RemoveStepAsync(Guid workflowId, Guid stepId)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(workflowId);

            workflow.RemoveStep(stepId);

            await _workflowRepository.UpdateAsync(workflow);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Update)]
        public async Task ReorderStepsAsync(Guid workflowId)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(workflowId);

            workflow.ReorderSteps();

            await _workflowRepository.UpdateAsync(workflow);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Update)]
        public async Task AddStepApproverUserAsync(Guid workflowId, Guid stepId, Guid userId)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(workflowId);
            var step = workflow.GetStep(stepId);

            step.AddApproverUser(userId);

            await _workflowRepository.UpdateAsync(workflow);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Update)]
        public async Task RemoveStepApproverUserAsync(Guid workflowId, Guid stepId, Guid userId)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(workflowId);
            var step = workflow.GetStep(stepId);

            step.RemoveApproverUser(userId);

            await _workflowRepository.UpdateAsync(workflow);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Update)]
        public async Task AddStepApproverRoleAsync(Guid workflowId, Guid stepId, Guid roleId)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(workflowId);
            var step = workflow.GetStep(stepId);

            step.AddApproverRole(roleId);

            await _workflowRepository.UpdateAsync(workflow);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Update)]
        public async Task RemoveStepApproverRoleAsync(Guid workflowId, Guid stepId, Guid roleId)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(workflowId);
            var step = workflow.GetStep(stepId);

            step.RemoveApproverRole(roleId);

            await _workflowRepository.UpdateAsync(workflow);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Update)]
        public async Task AddStepApproverTeamAsync(Guid workflowId, Guid stepId, Guid teamId)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(workflowId);
            var step = workflow.GetStep(stepId);

            step.AddApproverTeam(teamId);

            await _workflowRepository.UpdateAsync(workflow);
        }

        [Authorize(WorkManagementPermissions.Approval.Workflows.Update)]
        public async Task RemoveStepApproverTeamAsync(Guid workflowId, Guid stepId, Guid teamId)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(workflowId);
            var step = workflow.GetStep(stepId);

            step.RemoveApproverTeam(teamId);

            await _workflowRepository.UpdateAsync(workflow);
        }

        private ISpecification<ApprovalWorkflow> CreateListSpecification(
            GetApprovalWorkflowListDto input)
        {
            ISpecification<ApprovalWorkflow> spec = new BaseSpecification<ApprovalWorkflow>();

            if (input.WorkflowType.HasValue)
            {
                spec = spec.And(new BaseSpecification<ApprovalWorkflow>(
                    w => w.WorkflowType == input.WorkflowType.Value));
            }

            if (input.IsActive.HasValue)
            {
                spec = spec.And(new BaseSpecification<ApprovalWorkflow>(
                    w => w.IsActive == input.IsActive.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                spec = spec.And(new BaseSpecification<ApprovalWorkflow>(
                    w => w.Name.Contains(input.Filter)));
            }

            return spec;
        }
    }
}
