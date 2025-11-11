using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Infrastructure.Activity.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace NouAssist.Infrastructure.Activity.BackgroundJobs
{
    public class ActivityFeedAggregationJobArgs
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
    }

    public class ActivityFeedAggregationJob : AsyncBackgroundJob<ActivityFeedAggregationJobArgs>, ITransientDependency
    {
        private readonly IActivityFeedRepository _activityFeedRepository;

        public ActivityFeedAggregationJob(IActivityFeedRepository activityFeedRepository)
        {
            _activityFeedRepository = activityFeedRepository;
        }

        [UnitOfWork]
        public override async Task ExecuteAsync(ActivityFeedAggregationJobArgs args)
        {
            Logger.LogInformation($"Aggregating activity feed for user {args.UserId} on {args.Date:yyyy-MM-dd}");

            try
            {
                var spec = new Specification<Aggregates.ActivityFeed>(a => 
                    a.ActorId == args.UserId &&
                    a.OccurredAt.Date == args.Date.Date
                );

                var activities = await _activityFeedRepository.GetListAsync(spec, 1000);

                // Aggregate similar activities (e.g., "You uploaded 5 documents" instead of 5 separate activities)
                var aggregatedCount = activities
                    .GroupBy(a => new { a.Action, a.EntityType })
                    .Count(g => g.Count() > 1);

                Logger.LogInformation($"Found {activities.Count} activities, {aggregatedCount} groups can be aggregated");

                // TODO: Implement aggregation logic based on your business rules
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error aggregating activity feed for user {args.UserId}");
                throw;
            }
        }
    }
}
