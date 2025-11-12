using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Workflow
{
    public interface IWorkflowAppService : IApplicationService
    {
        Task<WorkflowDto> GetAsync(Guid id);
        Task<WorkflowDto> GetWithDetailsAsync(Guid id);
        Task<PagedResultDto<WorkflowDto>> GetListAsync(GetWorkflowListDto input);
        Task<WorkflowDto> CreateAsync(CreateWorkflowDto input);
        Task<WorkflowDto> UpdateAsync(Guid id, UpdateWorkflowDto input);
        Task DeleteAsync(Guid id);

        Task<WorkflowDto> SetAsDefaultAsync(Guid id);
        Task<WorkflowDto> ActivateAsync(Guid id);
        Task<WorkflowDto> DeactivateAsync(Guid id);

        Task<WorkflowDto> GetDefaultWorkflowAsync();
        Task<List<WorkflowTransitionDto>> GetAvailableTransitionsAsync(Guid workflowId, Guid currentStatusId);
    }
}
