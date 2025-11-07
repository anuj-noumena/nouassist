using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.ReflectionAndSupport.Reminders.Aggregates;
using NouAssist.ReflectionAndSupport.Reminders.Data;
using NouAssist.ReflectionAndSupport.Reminders.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace NouAssist.ReflectionAndSupport.Reminders.BackgroundJobs
{
    public class RecurringReminderCreationJobArgs
    {
        public Guid ParentReminderId { get; set; }
    }

    public class RecurringReminderCreationJob : AsyncBackgroundJob<RecurringReminderCreationJobArgs>, ITransientDependency
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly ReminderSchedulingService _schedulingService;

        public RecurringReminderCreationJob(
            IReminderRepository reminderRepository,
            ReminderSchedulingService schedulingService)
        {
            _reminderRepository = reminderRepository;
            _schedulingService = schedulingService;
        }

        [UnitOfWork]
        public override async Task ExecuteAsync(RecurringReminderCreationJobArgs args)
        {
            Logger.LogInformation($"Creating next occurrence for recurring reminder: {args.ParentReminderId}");

            try
            {
                var parentReminder = await _reminderRepository.GetAsync(args.ParentReminderId);

                if (!parentReminder.IsRecurring || parentReminder.IsCancelled)
                {
                    Logger.LogInformation($"Reminder {args.ParentReminderId} is not recurring or cancelled");
                    return;
                }

                // Use domain service to create next occurrence
                var nextOccurrence = _schedulingService.CreateNextOccurrence(parentReminder);

                if (nextOccurrence != null)
                {
                    await _reminderRepository.InsertAsync(nextOccurrence);
                    Logger.LogInformation($"Created next occurrence: {nextOccurrence.Id} for reminder {args.ParentReminderId}");
                }
                else
                {
                    Logger.LogInformation($"No more occurrences for recurring reminder: {args.ParentReminderId}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error creating next occurrence for reminder {args.ParentReminderId}");
                throw;
            }
        }
    }
}
