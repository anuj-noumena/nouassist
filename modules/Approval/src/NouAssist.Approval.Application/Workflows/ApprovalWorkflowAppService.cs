using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Data;
using NouAssist.Approval.Permissions;
using NouAssist.Approval.Specifications;
using NouAssist.Approval.Workflows.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Approval.Workflows
{
    [Authorize(ApprovalPermissions.Workflows.Default)]
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

            var workflows = await _workflowRepository.GetListAsync(spec);
            var totalCount = await _workflowRepository.CountAsync(spec);

            return new PagedResultDto<ApprovalWorkflowDto>(
                totalCount,
                ObjectMapper.Map<List<ApprovalWorkflow>, List<ApprovalWorkflowDto>>(workflows)
            );
        }

        [Authorize(ApprovalPermissions.Workflows.Create)]
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

            await _workflowRepository.InsertAsync(workflow);

            return ObjectMapper.Map<ApprovalWorkflow, ApprovalWorkflowDto>(workflow);
        }

        [Authorize(ApprovalPermissions.Workflows.Update)]
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

        [Authorize(ApprovalPermissions.Workflows.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _workflowRepository.DeleteAsync(id);
        }

        [Authorize(ApprovalPermissions.Workflows.Activate)]
        public async Task<ApprovalWorkflowDto> ActivateAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(id);
            
            workflow.Activate();
            
            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<ApprovalWorkflow, ApprovalWorkflowDto>(workflow);
        }

        [Authorize(ApprovalPermissions.Workflows.Deactivate)]
        public async Task<ApprovalWorkflowDto> DeactivateAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetAsync(id);
            
            workflow.Deactivate();
            
            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<ApprovalWorkflow, ApprovalWorkflowDto>(workflow);
        }

        [Authorize(ApprovalPermissions.Workflows.Update)]
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

            if (input.TimeoutHours.HasValue)
            {
                step.SetTimeout(input.TimeoutHours.Value, "escalate");
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

            step.ValidateApprovers();

            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<ApprovalStep, ApprovalStepDto>(step);
        }

        [Authorize(ApprovalPermissions.Workflows.Update)]
        public async Task RemoveStepAsync(Guid workflowId, Guid stepId)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(workflowId);

            workflow.RemoveStep(stepId);

            await _workflowRepository.UpdateAsync(workflow);
        }

        private ISpecification<ApprovalWorkflow> CreateListSpecification(
            GetApprovalWorkflowListDto input)
        {
            var spec = Specification.None<ApprovalWorkflow>();

            if (input.WorkflowType.HasValue)
            {
                spec = spec.And(new Specification<ApprovalWorkflow>(
                    w => w.WorkflowType == input.WorkflowType.Value));
            }

            if (input.IsActive.HasValue)
            {
                spec = spec.And(new Specification<ApprovalWorkflow>(
                    w => w.IsActive == input.IsActive.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                spec = spec.And(new Specification<ApprovalWorkflow>(
                    w => w.Name.Contains(input.Filter)));
            }

            return spec;
        }
    }
}
