using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Notification.Reminders.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace NouAssist.Notification.Reminders.BackgroundJobs
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
            Logger.LogInformation($"Processing reminder: {args.ReminderId}");

            try
            {
                var reminder = await _reminderRepository.GetAsync(args.ReminderId);

                if (reminder.IsSent || reminder.IsCancelled)
                {
                    Logger.LogInformation($"Reminder {args.ReminderId} already processed or cancelled");
                    return;
                }

                // Send notification via domain service
                await _notificationService.SendReminderNotificationAsync(reminder);

                // Mark as sent
                reminder.MarkAsSent();
                await _reminderRepository.UpdateAsync(reminder);

                Logger.LogInformation($"Successfully processed reminder: {args.ReminderId}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error processing reminder {args.ReminderId}");
                throw;
            }
        }
    }
}
