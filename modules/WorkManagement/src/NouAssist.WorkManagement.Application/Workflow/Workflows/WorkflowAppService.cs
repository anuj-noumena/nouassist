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

            // Note: Update logic would need to be added to the domain entity
            // For now, we can't update these private setters without domain methods

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
    }
}
