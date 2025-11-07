using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Workflows.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Workflows
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
