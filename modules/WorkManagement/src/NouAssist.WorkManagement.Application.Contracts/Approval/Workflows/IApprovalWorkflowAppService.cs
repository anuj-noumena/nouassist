using System;
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
        Task<ApprovalWorkflowDto> CreateAsync(CreateApprovalWorkflowDto input);
        Task<ApprovalWorkflowDto> UpdateAsync(Guid id, UpdateApprovalWorkflowDto input);
        Task DeleteAsync(Guid id);
        Task<ApprovalWorkflowDto> ActivateAsync(Guid id);
        Task<ApprovalWorkflowDto> DeactivateAsync(Guid id);
        Task<ApprovalStepDto> AddStepAsync(Guid workflowId, CreateApprovalStepDto input);
        Task RemoveStepAsync(Guid workflowId, Guid stepId);
    }
}
