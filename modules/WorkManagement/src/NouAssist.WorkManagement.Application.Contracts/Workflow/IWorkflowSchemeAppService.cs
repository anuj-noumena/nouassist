using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Workflow
{
    public interface IWorkflowSchemeAppService : IApplicationService
    {
        Task<WorkflowSchemeDto> GetAsync(Guid id);
        Task<WorkflowSchemeDto> GetWithMappingsAsync(Guid id);
        Task<PagedResultDto<WorkflowSchemeDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<WorkflowSchemeDto> CreateAsync(CreateWorkflowSchemeDto input);
        Task DeleteAsync(Guid id);

        Task<WorkflowSchemeDto> PublishAsync(Guid id);
        Task<WorkflowSchemeDto> ActivateAsync(Guid id);
        Task<WorkflowSchemeDto> DeactivateAsync(Guid id);

        Task<WorkflowSchemeDto> GetActiveSchemeForProjectAsync(Guid projectId);
        Task<Guid> GetWorkflowForTaskTypeAsync(Guid schemeId, Guid taskTypeId);
        
        // NEW: Basic updates
        Task<WorkflowSchemeDto> UpdateAsync(Guid id, UpdateWorkflowSchemeDto input);
        
        // NEW: Mapping management
        Task<WorkflowSchemeMappingDto> AddMappingAsync(CreateWorkflowSchemeMappingDto input);
        Task RemoveMappingAsync(Guid schemeId, Guid taskTypeId);
        Task<List<WorkflowSchemeMappingDto>> GetMappingsAsync(Guid schemeId);
        
        // NEW: Delegation settings
        Task<WorkflowSchemeDto> UpdateDelegationSettingsAsync(Guid id, bool allowQuickDelegation, bool requireApprovalForIntegrated);
    }
}
