using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Infrastructure.Reminders.Data;
using NouAssist.Infrastructure.Reminders.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace NouAssist.Infrastructure.Reminders.BackgroundJobs
{
    public class ReminderProcessingJobArgs
    {
        public Guid ReminderId { get; set; }
    }

    public class ReminderProcessingJob : AsyncBackgroundJob<ReminderProcessingJobArgs>, ITransientDependency
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly ReminderNotificationService _notificationService;

        public ReminderProcessingJob(
            IReminderRepository reminderRepository,
            ReminderNotificationService notificationService)
        {
            _reminderRepository = reminderRepository;
            _notificationService = notificationService;
        }

        [UnitOfWork]
        public override async Task ExecuteAsync(ReminderProcessingJobArgs args)
        {
            Logger.LogInformation("Processing reminder: {ReminderId}", args.ReminderId);

            try
            {
                var reminder = await _reminderRepository.GetAsync(args.ReminderId);

                if (reminder.IsSent || reminder.IsCancelled)
                {
                    Logger.LogInformation("Reminder {ReminderId} already processed or cancelled", args.ReminderId);
                    return;
                }

                // Send notification via domain service
                await _notificationService.SendReminderNotificationAsync(reminder);

                // Mark as sent
                reminder.MarkAsSent();
                await _reminderRepository.UpdateAsync(reminder);

                Logger.LogInformation("Successfully processed reminder: {ReminderId}", args.ReminderId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error processing reminder {ReminderId}", args.ReminderId);
                throw;
            }
        }
    }
}
