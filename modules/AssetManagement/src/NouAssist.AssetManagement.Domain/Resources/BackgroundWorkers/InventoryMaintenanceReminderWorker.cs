using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.AssetManagement.Resources.Data;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace NouAssist.AssetManagement.Resources.BackgroundWorkers
{
    public class InventoryMaintenanceReminderWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public InventoryMaintenanceReminderWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory)
            : base(timer, serviceScopeFactory)
        {
            Timer.Period = 86400000; // Run once per day
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("InventoryMaintenanceReminderWorker: Checking for items due for maintenance...");

            var inventoryRepository = workerContext.ServiceProvider.GetRequiredService<IInventoryItemRepository>();

            var itemsDue = await inventoryRepository.GetItemsDueForMaintenanceAsync();

            Logger.LogInformation($"Found {itemsDue.Count} items due for maintenance");

            foreach (var item in itemsDue)
            {
                // TODO: Send notification/reminder to owner
                Logger.LogInformation($"Item {item.Id} ({item.Name}) is due for maintenance");
            }
        }
    }
}
