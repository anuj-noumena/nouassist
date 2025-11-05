using System;
using System.Threading.Tasks;
using NouAssist.Core.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Core.Inbox
{
    public interface IInboxItemAppService : IApplicationService
    {
        Task<InboxItemDto> GetAsync(Guid id);
        Task<ListResultDto<InboxItemDto>> GetMyUnprocessedItemsAsync();
        Task<ListResultDto<InboxItemDto>> GetStaleItemsAsync(int daysToConsiderStale = 7);
        Task<int> GetMyUnprocessedCountAsync();
        Task<InboxItemDto> CreateAsync(CreateInboxItemDto input);
        Task<InboxItemDto> UpdateAsync(Guid id, UpdateInboxItemDto input);
        Task DeleteAsync(Guid id);
        Task<InboxItemDto> ProcessAsync(Guid id, ProcessInboxItemDto input);
    }
}
