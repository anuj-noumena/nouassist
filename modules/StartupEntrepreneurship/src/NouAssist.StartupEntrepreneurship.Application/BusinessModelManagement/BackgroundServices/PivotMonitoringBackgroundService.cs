using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BackgroundServices
{
    public class PivotMonitoringBackgroundService : AsyncPeriodicBackgroundWorkerBase
    {
        public PivotMonitoringBackgroundService(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory)
            : base(timer, serviceScopeFactory)
        {
            Timer.Period = 3600000; // 1 hour
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Checking for overdue strategic pivots...");

            using var scope = ServiceScopeFactory.CreateScope();
            var pivotRepository = scope.ServiceProvider.GetRequiredService<IStrategicPivotRepository>();
            
            var overduePivots = await pivotRepository.GetOverduePivotsAsync(DateTime.UtcNow, null);
            
            if (overduePivots.Any())
            {
                Logger.LogWarning("Found {Count} overdue strategic pivots", overduePivots.Count);
                // Here you could send notifications, create alerts, etc.
            }
        }
    }
}
