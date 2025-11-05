using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Activity.ActivityFeeds.Dtos;
using NouAssist.Activity.Aggregates;
using NouAssist.Activity.Data;
using NouAssist.Activity.Permissions;
using NouAssist.Activity.Specifications;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Activity.ActivityFeeds
{
    [Authorize(ActivityPermissions.ActivityFeeds.Default)]
    public class ActivityFeedAppService : ApplicationService, IActivityFeedAppService
    {
        private readonly IActivityFeedRepository _activityFeedRepository;

        public ActivityFeedAppService(IActivityFeedRepository activityFeedRepository)
        {
            _activityFeedRepository = activityFeedRepository;
        }

        public async Task<ActivityFeedDto> GetAsync(Guid id)
        {
            var activity = await _activityFeedRepository.GetAsync(id);
            return ObjectMapper.Map<ActivityFeed, ActivityFeedDto>(activity);
        }

        public async Task<PagedResultDto<ActivityFeedDto>> GetListAsync(GetActivityFeedListDto input)
        {
            var spec = CreateListSpecification(input);
            
            var activities = await _activityFeedRepository.GetListAsync(spec, input.MaxResultCount);
            var totalCount = await _activityFeedRepository.GetCountAsync(spec);

            return new PagedResultDto<ActivityFeedDto>(
                totalCount,
                ObjectMapper.Map<List<ActivityFeed>, List<ActivityFeedDto>>(activities)
            );
        }

        public async Task<int> GetUnreadCountAsync()
        {
            return await _activityFeedRepository.GetUnreadCountAsync(CurrentUser.Id.Value);
        }

        public async Task MarkAsReadAsync(Guid id)
        {
            var activity = await _activityFeedRepository.GetAsync(id);
            activity.MarkAsRead();
            await _activityFeedRepository.UpdateAsync(activity);
        }

        public async Task MarkAllAsReadAsync()
        {
            var spec = new UnreadActivitiesSpecification(CurrentUser.Id.Value);
            var unreadActivities = await _activityFeedRepository.GetListAsync(spec, 100);

            foreach (var activity in unreadActivities)
            {
                activity.MarkAsRead();
            }

            await _activityFeedRepository.UpdateManyAsync(unreadActivities);
        }

        private ISpecification<ActivityFeed> CreateListSpecification(GetActivityFeedListDto input)
        {
            var spec = Specification.True<ActivityFeed>();

            if (input.ActorId.HasValue)
            {
                spec = spec.And(new Specification<ActivityFeed>(a => a.ActorId == input.ActorId.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.EntityType))
            {
                spec = spec.And(new Specification<ActivityFeed>(a => a.EntityType.ToString() == input.EntityType));
            }

            if (input.EntityId.HasValue)
            {
                spec = spec.And(new Specification<ActivityFeed>(a => a.EntityId == input.EntityId.Value));
            }

            if (input.FromDate.HasValue)
            {
                spec = spec.And(new Specification<ActivityFeed>(a => a.OccurredAt >= input.FromDate.Value));
            }

            if (input.ToDate.HasValue)
            {
                spec = spec.And(new Specification<ActivityFeed>(a => a.OccurredAt <= input.ToDate.Value));
            }

            if (input.IsRead.HasValue)
            {
                spec = spec.And(new Specification<ActivityFeed>(a => a.IsRead == input.IsRead.Value));
            }

            return spec;
        }
    }
}
