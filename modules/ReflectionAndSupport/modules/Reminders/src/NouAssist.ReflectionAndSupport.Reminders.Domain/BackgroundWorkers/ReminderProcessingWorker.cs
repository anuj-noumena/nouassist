using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.ReflectionAndSupport.Reminders.BackgroundJobs;
using NouAssist.ReflectionAndSupport.Reminders.Data;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace NouAssist.ReflectionAndSupport.Reminders.BackgroundWorkers
{
    public class ReminderProcessingWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public ReminderProcessingWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory)
            : base(timer, serviceScopeFactory)
        {
            Timer.Period = 60000; // Run every 1 minute
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("ReminderProcessingWorker: Checking for due reminders...");

            var reminderRepository = workerContext.ServiceProvider.GetRequiredService<IReminderRepository>();
            var backgroundJobManager = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobManager>();

            // Get all due reminders
            var dueReminders = await reminderRepository.GetDueRemindersAsync(DateTime.UtcNow);

            Logger.LogInformation($"Found {dueReminders.Count} due reminders");

            foreach (var reminder in dueReminders)
            {
                try
                {
                    // Queue background job for each reminder
                    await backgroundJobManager.EnqueueAsync(
                        new ReminderProcessingJobArgs { ReminderId = reminder.Id }
                    );

                    Logger.LogInformation($"Queued processing job for reminder: {reminder.Id}");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Error queuing job for reminder {reminder.Id}");
                }
            }
        }
    }
}
