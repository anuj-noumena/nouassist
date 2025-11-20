using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Infrastructure.Activity.Aggregates;
using NouAssist.Infrastructure.Activity.ActivityFeeds.Dtos;
using NouAssist.Infrastructure.Activity.Data;
using NouAssist.Infrastructure.Activity.Specifications;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using Volo.Abp.Users;

namespace NouAssist.Infrastructure.Activity.ActivityFeeds
{
    public class ActivityFeedAppService : ApplicationService, IActivityFeedAppService
    {
        private readonly IActivityFeedRepository _activityFeedRepository;
        private readonly IAsyncQueryableExecuter _asyncExecuter;
        private readonly ICurrentUser _currentUser;

        public ActivityFeedAppService(
            IActivityFeedRepository activityFeedRepository,
            IAsyncQueryableExecuter asyncExecuter, ICurrentUser currentUser)
        {
            _activityFeedRepository = activityFeedRepository;
            _asyncExecuter = asyncExecuter;
            _currentUser = currentUser;
        }

        public async Task<ActivityFeedDto> GetAsync(Guid id)
        {
            var activityFeed = await _activityFeedRepository.GetAsync(id);
            return ObjectMapper.Map<ActivityFeed, ActivityFeedDto>(activityFeed);
        }

        public async Task<PagedResultDto<ActivityFeedDto>> GetListAsync(GetActivityFeedListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(ActivityFeed.OccurredAt) + " DESC";
            }

            var query = new ActivitiesSpecification(_currentUser.Id.Value, input.IsRead, input.FromDate, input.ToDate);
            var items = await _activityFeedRepository.GetListAsync(query);

            var totalCount = await _activityFeedRepository.CountAsync(query);

            return new PagedResultDto<ActivityFeedDto>(
                totalCount,
                ObjectMapper.Map<List<ActivityFeed>, List<ActivityFeedDto>>(items)
            );
        }

        public async Task<int> GetUnreadCountAsync()
        {
            return await _activityFeedRepository.CountAsync(x => x.ActorId == CurrentUser.Id && !x.IsRead);
        }

        public async Task MarkAsReadAsync(Guid id)
        {
            var activityFeed = await _activityFeedRepository.GetAsync(id);
            if (activityFeed != null)
            {
                activityFeed.MarkAsRead();
                await _activityFeedRepository.UpdateAsync(activityFeed);
            }
        }

        public async Task MarkAllAsReadAsync()
        {
            var unreadItems = await _activityFeedRepository.GetListAsync(x => x.ActorId == CurrentUser.Id && !x.IsRead);
            foreach (var item in unreadItems)
            {
                item.MarkAsRead();
                await _activityFeedRepository.UpdateAsync(item);
            }
        }
    }
}