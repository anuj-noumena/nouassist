using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Data;
using NouAssist.WorkManagement.Workflow.Dtos;
using NouAssist.WorkManagement.Workflow.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Workflow.Workflows
{
    [Authorize(WorkManagementPermissions.Workflow.Workflows.Default)]
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

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Create)]
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

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _workflowSchemeRepository.DeleteAsync(id);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowSchemeDto> PublishAsync(Guid id)
        {
            var scheme = await _workflowSchemeRepository.GetAsync(id);
            scheme.Publish();
            await _workflowSchemeRepository.UpdateAsync(scheme);

            return ObjectMapper.Map<WorkflowScheme, WorkflowSchemeDto>(scheme);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowSchemeDto> ActivateAsync(Guid id)
        {
            var scheme = await _workflowSchemeRepository.GetAsync(id);
            scheme.Activate();
            await _workflowSchemeRepository.UpdateAsync(scheme);

            return ObjectMapper.Map<WorkflowScheme, WorkflowSchemeDto>(scheme);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
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

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowSchemeDto> UpdateAsync(Guid id, UpdateWorkflowSchemeDto input)
        {
            var scheme = await _workflowSchemeRepository.GetAsync(id);

            scheme.UpdateBasicInfo(input.Name, input.Description);

            if (input.DefaultWorkflowId.HasValue)
            {
                scheme.SetDefaultWorkflow(input.DefaultWorkflowId.Value);
            }

            scheme.SetDelegationMode(input.AllowQuickDelegation, input.RequireApprovalForIntegrated);

            await _workflowSchemeRepository.UpdateAsync(scheme);

            return ObjectMapper.Map<WorkflowScheme, WorkflowSchemeDto>(scheme);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Create)]
        public async Task<WorkflowSchemeMappingDto> AddMappingAsync(CreateWorkflowSchemeMappingDto input)
        {
            var scheme = await _workflowSchemeRepository.GetWithMappingsAsync(input.WorkflowSchemeId);

            var mapping = new Entities.WorkflowSchemeMapping(
                GuidGenerator.Create(),
                input.WorkflowSchemeId,
                input.TaskTypeId,
                input.WorkflowId
            );

            scheme.AddMapping(mapping);
            await _workflowSchemeRepository.UpdateAsync(scheme);

            return ObjectMapper.Map<Entities.WorkflowSchemeMapping, WorkflowSchemeMappingDto>(mapping);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Delete)]
        public async Task RemoveMappingAsync(Guid schemeId, Guid taskTypeId)
        {
            var scheme = await _workflowSchemeRepository.GetWithMappingsAsync(schemeId);
            scheme.RemoveMapping(taskTypeId);
            await _workflowSchemeRepository.UpdateAsync(scheme);
        }

        public async Task<List<WorkflowSchemeMappingDto>> GetMappingsAsync(Guid schemeId)
        {
            var scheme = await _workflowSchemeRepository.GetWithMappingsAsync(schemeId);
            return ObjectMapper.Map<List<Entities.WorkflowSchemeMapping>, List<WorkflowSchemeMappingDto>>(
                scheme.Mappings.ToList());
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowSchemeDto> UpdateDelegationSettingsAsync(
            Guid id,
            bool allowQuickDelegation,
            bool requireApprovalForIntegrated)
        {
            var scheme = await _workflowSchemeRepository.GetAsync(id);
            scheme.SetDelegationMode(allowQuickDelegation, requireApprovalForIntegrated);
            await _workflowSchemeRepository.UpdateAsync(scheme);

            return ObjectMapper.Map<WorkflowScheme, WorkflowSchemeDto>(scheme);
        }
    }
}