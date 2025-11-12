using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.GTD
{
    public interface IWaitingForAppService : IApplicationService
    {
        Task<WaitingForDto> GetAsync(Guid id);
        Task<ListResultDto<WaitingForDto>> GetMyActiveItemsAsync();
        Task<ListResultDto<WaitingForDto>> GetFollowUpDueAsync();
        Task<WaitingForDto> CreateAsync(CreateWaitingForDto input);
        Task<WaitingForDto> UpdateAsync(Guid id, UpdateWaitingForDto input);
        Task DeleteAsync(Guid id);
        Task<WaitingForDto> ResolveAsync(Guid id);
        Task<WaitingForDto> PutOnHoldAsync(Guid id);
        Task<WaitingForDto> ReactivateAsync(Guid id);
        Task<WaitingForDto> CancelAsync(Guid id);
        Task<WaitingForDto> EscalateAsync(Guid id);
    }

    public interface IFollowupAppService : IApplicationService
    {
        Task<FollowupDto> GetAsync(Guid id);
        Task<ListResultDto<FollowupDto>> GetMyDueFollowupsAsync();
        Task<FollowupDto> CreateAsync(CreateFollowupDto input);
        Task<FollowupDto> MarkAsDoneAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
