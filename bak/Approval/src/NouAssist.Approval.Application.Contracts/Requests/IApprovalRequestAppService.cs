using System;
using System.Threading.Tasks;
using NouAssist.Approval.Requests.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Approval.Requests
{
    public interface IApprovalRequestAppService : IApplicationService
    {
        Task<ApprovalRequestDto> GetAsync(Guid id);
        Task<PagedResultDto<ApprovalRequestDto>> GetListAsync(GetApprovalRequestListDto input);
        Task<PagedResultDto<ApprovalRequestDto>> GetMyRequestsAsync(GetApprovalRequestListDto input);
        Task<PagedResultDto<ApprovalRequestDto>> GetPendingApprovalsAsync(GetApprovalRequestListDto input);
        Task<ApprovalRequestDto> CreateAsync(CreateApprovalRequestDto input);
        Task<ApprovalRequestDto> MakeDecisionAsync(Guid id, MakeApprovalDecisionDto input);
        Task<ApprovalRequestDto> CancelAsync(Guid id, string reason);
    }
}
