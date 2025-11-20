using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.Infrastructure.Search.Services;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace NouAssist.Infrastructure.Search.BackgroundWorkers
{
    public class SearchAnalyticsAggregationWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public SearchAnalyticsAggregationWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory)
            : base(timer, serviceScopeFactory)
        {
            Timer.Period = 1800000; // Run every 30 minutes
        }

        [UnitOfWork]
        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("SearchAnalyticsAggregationWorker: Aggregating search analytics...");

            var searchAnalyticsService = workerContext.ServiceProvider.GetRequiredService<SearchAnalyticsService>();

            // Update popular searches based on recent history (last hour)
            await searchAnalyticsService.UpdatePopularSearchesAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow);

            Logger.LogInformation("SearchAnalyticsAggregationWorker: Completed aggregation");
        }
    }
}
