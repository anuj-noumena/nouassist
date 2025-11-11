using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Data;
using NouAssist.WorkManagement.Workflow.Permissions;
using NouAssist.WorkManagement.Workflow.Workflows.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Workflow.Workflows
{
    [Authorize(WorkflowPermissions.Workflows.Default)]
    public class WorkflowSchemeAppService : ApplicationService, IWorkflowSchemeAppService
    {
        private readonly IWorkflowSchemeRepository _workflowSchemeRepository;

        public WorkflowSchemeAppService(IWorkflowSchemeRepository workflowSchemeRepository)
        {
            _workflowSchemeRepository = workflowSchemeRepository;
        }

        public async Task<WorkflowSchemeDto> GetAsync(Guid id)
        {
            var scheme = await _workflowSchemeRepository.GetAsync(id);
            return ObjectMapper.Map<WorkflowScheme, WorkflowSchemeDto>(scheme);
        }

        public async Task<WorkflowSchemeDto> GetWithMappingsAsync(Guid id)
        {
            var scheme = await _workflowSchemeRepository.GetWithMappingsAsync(id);
            return ObjectMapper.Map<WorkflowScheme, WorkflowSchemeDto>(scheme);
        }

        public async Task<PagedResultDto<WorkflowSchemeDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _workflowSchemeRepository.GetQueryableAsync();

            var totalCount = queryable.Count();

            // Apply sorting
            queryable = !string.IsNullOrWhiteSpace(input.Sorting)
                ? queryable.OrderBy(input.Sorting)
                : queryable.OrderBy(ws => ws.Name);

            // Apply paging
            var schemes = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<WorkflowSchemeDto>(
                totalCount,
                ObjectMapper.Map<List<WorkflowScheme>, List<WorkflowSchemeDto>>(schemes)
            );
        }

        [Authorize(WorkflowPermissions.Workflows.Create)]
        public async Task<WorkflowSchemeDto> CreateAsync(CreateWorkflowSchemeDto input)
        {
            var scheme = new WorkflowScheme(
                GuidGenerator.Create(),
                input.Name,
                input.ProjectId,
                input.Description,
                input.DefaultWorkflowId
            );

            await _workflowSchemeRepository.InsertAsync(scheme);

            return ObjectMapper.Map<WorkflowScheme, WorkflowSchemeDto>(scheme);
        }

        [Authorize(WorkflowPermissions.Workflows.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _workflowSchemeRepository.DeleteAsync(id);
        }

        [Authorize(WorkflowPermissions.Workflows.Update)]
        public async Task<WorkflowSchemeDto> PublishAsync(Guid id)
        {
            var scheme = await _workflowSchemeRepository.GetAsync(id);
            scheme.Publish();
            await _workflowSchemeRepository.UpdateAsync(scheme);

            return ObjectMapper.Map<WorkflowScheme, WorkflowSchemeDto>(scheme);
        }

        [Authorize(WorkflowPermissions.Workflows.Update)]
        public async Task<WorkflowSchemeDto> ActivateAsync(Guid id)
        {
            var scheme = await _workflowSchemeRepository.GetAsync(id);
            scheme.Activate();
            await _workflowSchemeRepository.UpdateAsync(scheme);

            return ObjectMapper.Map<WorkflowScheme, WorkflowSchemeDto>(scheme);
        }

        [Authorize(WorkflowPermissions.Workflows.Update)]
        public async Task<WorkflowSchemeDto> DeactivateAsync(Guid id)
        {
            var scheme = await _workflowSchemeRepository.GetAsync(id);
            scheme.Deactivate();
            await _workflowSchemeRepository.UpdateAsync(scheme);

            return ObjectMapper.Map<WorkflowScheme, WorkflowSchemeDto>(scheme);
        }

        public async Task<WorkflowSchemeDto> GetActiveSchemeForProjectAsync(Guid projectId)
        {
            var scheme = await _workflowSchemeRepository.GetActiveSchemeForProjectAsync(projectId);
            return ObjectMapper.Map<WorkflowScheme, WorkflowSchemeDto>(scheme);
        }

        public async Task<Guid> GetWorkflowForTaskTypeAsync(Guid schemeId, Guid taskTypeId)
        {
            var scheme = await _workflowSchemeRepository.GetWithMappingsAsync(schemeId);
            return scheme.GetWorkflowForTaskType(taskTypeId);
        }
    }
}
