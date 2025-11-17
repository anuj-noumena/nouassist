using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.Workflow.Data;
using NouAssist.WorkManagement.Workflow.Dtos;
using NouAssist.WorkManagement.Workflow.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Workflow.Workflows
{
    [Authorize(WorkManagementPermissions.Workflow.Workflows.Default)]
    public class WorkflowAppService : ApplicationService, IWorkflowAppService
    {
        private readonly IWorkflowRepository _workflowRepository;

        public WorkflowAppService(IWorkflowRepository workflowRepository)
        {
            _workflowRepository = workflowRepository;
        }

        public async Task<WorkflowDto> GetAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetAsync(id);
            return ObjectMapper.Map<Aggregates.Workflow, WorkflowDto>(workflow);
        }

        public async Task<WorkflowDto> GetWithDetailsAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetWithDetailsAsync(id);
            return ObjectMapper.Map<Aggregates.Workflow, WorkflowDto>(workflow);
        }

        public async Task<PagedResultDto<WorkflowDto>> GetListAsync(GetWorkflowListDto input)
        {
            var queryable = await _workflowRepository.GetQueryableAsync();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(w =>
                    w.Name.Contains(input.Filter) ||
                    (w.Description != null && w.Description.Contains(input.Filter)));
            }

            if (input.IsActive.HasValue)
            {
                queryable = queryable.Where(w => w.IsActive == input.IsActive.Value);
            }

            if (input.IsDefault.HasValue)
            {
                queryable = queryable.Where(w => w.IsDefault == input.IsDefault.Value);
            }

            var totalCount = queryable.Count();

            // Apply sorting
            queryable = !string.IsNullOrWhiteSpace(input.Sorting)
                ? queryable.OrderBy(input.Sorting)
                : queryable.OrderBy(w => w.Name);

            // Apply paging
            var workflows = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<WorkflowDto>(
                totalCount,
                ObjectMapper.Map<List<Aggregates.Workflow>, List<WorkflowDto>>(workflows)
            );
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Create)]
        public async Task<WorkflowDto> CreateAsync(CreateWorkflowDto input)
        {
            var workflow = new Aggregates.Workflow(
                GuidGenerator.Create(),
                input.Name,
                input.InitialStatusId,
                input.Description
            );

            await _workflowRepository.InsertAsync(workflow);

            return ObjectMapper.Map<Aggregates.Workflow, WorkflowDto>(workflow);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowDto> UpdateAsync(Guid id, UpdateWorkflowDto input)
        {
            var workflow = await _workflowRepository.GetAsync(id);

            // Use domain methods for updates
            workflow.UpdateBasicInfo(input.Name, input.Description);
            workflow.UpdateInitialStatus(input.InitialStatusId);

            if (input.IsActive)
            {
                workflow.Activate();
            }
            else
            {
                workflow.Deactivate();
            }

            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<Aggregates.Workflow, WorkflowDto>(workflow);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _workflowRepository.DeleteAsync(id);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowDto> SetAsDefaultAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetAsync(id);
            workflow.SetAsDefault();
            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<Aggregates.Workflow, WorkflowDto>(workflow);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowDto> ActivateAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetAsync(id);
            workflow.Activate();
            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<Aggregates.Workflow, WorkflowDto>(workflow);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowDto> DeactivateAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetAsync(id);
            workflow.Deactivate();
            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<Aggregates.Workflow, WorkflowDto>(workflow);
        }

        public async Task<WorkflowDto> GetDefaultWorkflowAsync()
        {
            var workflow = await _workflowRepository.GetDefaultWorkflowAsync();
            return ObjectMapper.Map<Aggregates.Workflow, WorkflowDto>(workflow);
        }

        public async Task<List<WorkflowTransitionDto>> GetAvailableTransitionsAsync(
            Guid workflowId,
            Guid currentStatusId)
        {
            var workflow = await _workflowRepository.GetWithDetailsAsync(workflowId);
            var transitions = workflow.GetAvailableTransitions(currentStatusId);

            return ObjectMapper.Map<List<Entities.WorkflowTransition>, List<WorkflowTransitionDto>>(transitions);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Create)]
        public async Task<WorkflowStatusDto> AddStatusAsync(CreateWorkflowStatusDto input)
        {
            var workflow = await _workflowRepository.GetWithDetailsAsync(input.WorkflowId);

            var status = new Entities.WorkflowStatus(
                GuidGenerator.Create(),
                input.WorkflowId,
                input.Name,
                input.Category,
                input.Color,
                input.OrderIndex,
                input.IsInitial
            )
            {
                Description = input.Description,
                IsResolution = input.IsResolution
            };

            workflow.AddStatus(status);
            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<Entities.WorkflowStatus, WorkflowStatusDto>(status);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowStatusDto> UpdateStatusAsync(Guid id, UpdateWorkflowStatusDto input)
        {
            var workflow = await _workflowRepository.GetWithDetailsAsync(id);
            var status = workflow.Statuses.FirstOrDefault(s => s.Id == id);

            if (status == null)
            {
                throw new BusinessException("Workflow:StatusNotFound");
            }

            status.UpdateInfo(input.Name, input.Description, input.Color);
            status.OrderIndex = input.OrderIndex;
            status.IsResolution = input.IsResolution;

            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<Entities.WorkflowStatus, WorkflowStatusDto>(status);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Delete)]
        public async Task RemoveStatusAsync(Guid workflowId, Guid statusId)
        {
            var workflow = await _workflowRepository.GetWithDetailsAsync(workflowId);
            workflow.RemoveStatus(statusId);
            await _workflowRepository.UpdateAsync(workflow);
        }

        public async Task<WorkflowStatusDto> GetStatusAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetWithDetailsAsync(id);
            var status = workflow.GetStatus(id);
            return ObjectMapper.Map<Entities.WorkflowStatus, WorkflowStatusDto>(status);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Create)]
        public async Task<WorkflowTransitionDto> AddTransitionAsync(CreateWorkflowTransitionDto input)
        {
            var workflow = await _workflowRepository.GetWithDetailsAsync(input.WorkflowId);

            var transition = new Entities.WorkflowTransition(
                GuidGenerator.Create(),
                input.WorkflowId,
                input.Name,
                input.FromStatusId,
                input.ToStatusId,
                input.Purpose,
                input.Description
            )
            {
                RequiresComment = input.RequiresComment,
                RequiresResolution = input.RequiresResolution,
                IsGlobal = input.IsGlobal,
                OrderIndex = input.OrderIndex,
                ButtonLabel = input.ButtonLabel,
                ButtonColor = input.ButtonColor
            };

            workflow.AddTransition(transition);
            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<Entities.WorkflowTransition, WorkflowTransitionDto>(transition);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowTransitionDto> UpdateTransitionAsync(Guid id, UpdateWorkflowTransitionDto input)
        {
            var workflow = await _workflowRepository.GetWithDetailsAsync(id);
            var transition = workflow.Transitions.FirstOrDefault(t => t.Id == id);

            if (transition == null)
            {
                throw new BusinessException("Workflow:TransitionNotFound");
            }

            // Update transition properties
            // Note: WorkflowTransition needs update methods in domain
            transition.SetPurpose(input.Purpose);
            transition.RequiresComment = input.RequiresComment;
            transition.RequiresResolution = input.RequiresResolution;
            transition.IsGlobal = input.IsGlobal;
            transition.OrderIndex = input.OrderIndex;
            transition.ButtonLabel = input.ButtonLabel;
            transition.ButtonColor = input.ButtonColor;

            await _workflowRepository.UpdateAsync(workflow);

            return ObjectMapper.Map<Entities.WorkflowTransition, WorkflowTransitionDto>(transition);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Delete)]
        public async Task RemoveTransitionAsync(Guid workflowId, Guid transitionId)
        {
            var workflow = await _workflowRepository.GetWithDetailsAsync(workflowId);
            workflow.RemoveTransition(transitionId);
            await _workflowRepository.UpdateAsync(workflow);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task ValidateWorkflowAsync(Guid workflowId)
        {
            var workflow = await _workflowRepository.GetWithDetailsAsync(workflowId);
            workflow.ValidateBeforeActivation();
        }
    }
}