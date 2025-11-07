using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.Search.Data;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace NouAssist.Search.BackgroundWorkers
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

            var popularSearchRepository = workerContext.ServiceProvider.GetRequiredService<IPopularSearchRepository>();

            // Update popular searches based on recent history
            await popularSearchRepository.UpdatePopularSearchesAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow);

            Logger.LogInformation("SearchAnalyticsAggregationWorker: Completed aggregation");
        }
    }
}
