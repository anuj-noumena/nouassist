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
        
        // NEW: Status management
        Task<WorkflowStatusDto> AddStatusAsync(CreateWorkflowStatusDto input);
        Task<WorkflowStatusDto> UpdateStatusAsync(Guid id, UpdateWorkflowStatusDto input);
        Task RemoveStatusAsync(Guid workflowId, Guid statusId);
        Task<WorkflowStatusDto> GetStatusAsync(Guid id);
        
        // NEW: Transition management
        Task<WorkflowTransitionDto> AddTransitionAsync(CreateWorkflowTransitionDto input);
        Task<WorkflowTransitionDto> UpdateTransitionAsync(Guid id, UpdateWorkflowTransitionDto input);
        Task RemoveTransitionAsync(Guid workflowId, Guid transitionId);
        
        // NEW: Validation
        Task ValidateWorkflowAsync(Guid workflowId);
    }
}
