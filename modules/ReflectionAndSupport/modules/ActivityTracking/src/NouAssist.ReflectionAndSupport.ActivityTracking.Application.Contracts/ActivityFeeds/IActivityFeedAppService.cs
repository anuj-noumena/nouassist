using System;
using System.Threading.Tasks;
using NouAssist.ReflectionAndSupport.ActivityTracking.ActivityFeeds.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.ActivityFeeds
{
    public interface IActivityFeedAppService : IApplicationService
    {
        Task<ActivityFeedDto> GetAsync(Guid id);
        Task<PagedResultDto<ActivityFeedDto>> GetListAsync(GetActivityFeedListDto input);
        Task<int> GetUnreadCountAsync();
        Task MarkAsReadAsync(Guid id);
        Task MarkAllAsReadAsync();
    }
}
