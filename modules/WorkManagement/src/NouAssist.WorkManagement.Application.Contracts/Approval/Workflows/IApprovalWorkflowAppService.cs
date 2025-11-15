using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Approval.Workflows.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Approval.Workflows
{
    public interface IApprovalWorkflowAppService : IApplicationService
    {
        Task<ApprovalWorkflowDto> GetAsync(Guid id);
        Task<PagedResultDto<ApprovalWorkflowDto>> GetListAsync(GetApprovalWorkflowListDto input);
        Task<List<ApprovalWorkflowDto>> GetActiveWorkflowsAsync();
        Task<List<ApprovalWorkflowDto>> GetByTypeAsync(int workflowType);
        Task<ApprovalWorkflowDto> CreateAsync(CreateApprovalWorkflowDto input);
        Task<ApprovalWorkflowDto> UpdateAsync(Guid id, UpdateApprovalWorkflowDto input);
        Task DeleteAsync(Guid id);
        Task<ApprovalWorkflowDto> ActivateAsync(Guid id);
        Task<ApprovalWorkflowDto> DeactivateAsync(Guid id);
        
        // Step management
        Task<ApprovalStepDto> AddStepAsync(Guid workflowId, CreateApprovalStepDto input);
        Task<ApprovalStepDto> UpdateStepAsync(Guid workflowId, Guid stepId, UpdateApprovalStepDto input);
        Task RemoveStepAsync(Guid workflowId, Guid stepId);
        Task ReorderStepsAsync(Guid workflowId);
        
        // Approver management
        Task AddStepApproverUserAsync(Guid workflowId, Guid stepId, Guid userId);
        Task RemoveStepApproverUserAsync(Guid workflowId, Guid stepId, Guid userId);
        Task AddStepApproverRoleAsync(Guid workflowId, Guid stepId, Guid roleId);
        Task RemoveStepApproverRoleAsync(Guid workflowId, Guid stepId, Guid roleId);
        Task AddStepApproverTeamAsync(Guid workflowId, Guid stepId, Guid teamId);
        Task RemoveStepApproverTeamAsync(Guid workflowId, Guid stepId, Guid teamId);
    }
}