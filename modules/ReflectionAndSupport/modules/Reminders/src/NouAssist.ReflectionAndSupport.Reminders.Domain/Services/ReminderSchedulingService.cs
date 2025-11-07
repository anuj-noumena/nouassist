using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.ReflectionAndSupport.Reminders.Aggregates;
using NouAssist.ReflectionAndSupport.Reminders.Data;
using NouAssist.ReflectionAndSupport.Reminders.Enums;
using NouAssist.ReflectionAndSupport.Reminders.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.ReflectionAndSupport.Reminders.Services
{
    public class ReminderSchedulingService : DomainService
    {
        private readonly IReminderRepository _reminderRepository;

        public ReminderSchedulingService(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public Reminder CreateSimpleReminder(
            Guid ownerId,
            string title,
            DateTime remindAt,
            Guid? tenantId,
            ReminderEntityType? entityType = null,
            Guid? entityId = null,
            ReminderPriority priority = ReminderPriority.Normal)
        {
            var reminder = new Reminder(
                GuidGenerator.Create(),
                ownerId,
                title,
                remindAt,
                tenantId,
                entityType,
                entityId,
                priority
            );

            return reminder;
        }

        public Reminder CreateRecurringReminder(
            Guid ownerId,
            string title,
            DateTime baseDateTime,
            int minutesBeforeEvent,
            RecurrencePattern recurrencePattern,
            Guid? tenantId,
            ReminderEntityType? entityType = null,
            Guid? entityId = null,
            int? recurrenceInterval = null,
            DateTime? recurrenceEndDate = null,
            List<DayOfWeek> recurrenceDaysOfWeek = null,
            ReminderPriority priority = ReminderPriority.Normal)
        {
            var schedule = new ReminderSchedule(
                baseDateTime,
                minutesBeforeEvent,
                recurrencePattern,
                recurrenceInterval,
                recurrenceEndDate,
                recurrenceDaysOfWeek
            );

            var reminder = new Reminder(
                GuidGenerator.Create(),
                ownerId,
                title,
                schedule.CalculateReminderTime(),
                tenantId,
                entityType,
                entityId,
                priority
            );

            reminder.SetSchedule(schedule);

            return reminder;
        }

        public async Task<List<Reminder>> GetDueRemindersAsync(DateTime upToDateTime)
        {
            return await _reminderRepository.GetDueRemindersAsync(upToDateTime);
        }

        public async Task<List<Reminder>> GetUpcomingRemindersAsync(Guid userId, int hoursAhead = 24)
        {
            var startTime = Clock.Now;
            var endTime = startTime.AddHours(hoursAhead);
            
            return await _reminderRepository.GetUpcomingRemindersAsync(userId, startTime, endTime);
        }

        public void ProcessDueReminder(Reminder reminder)
        {
            if (reminder.IsCancelled)
                return;

            if (!reminder.IsDue && !reminder.IsOverdue)
                return;

            reminder.MarkAsSent();
        }

        public void SnoozeReminder(Reminder reminder, int minutes)
        {
            if (minutes <= 0)
                minutes = GetDefaultSnoozeDuration(reminder.Priority);

            reminder.Snooze(minutes);
        }

        private int GetDefaultSnoozeDuration(ReminderPriority priority)
        {
            return priority switch
            {
                ReminderPriority.Critical => 5,
                ReminderPriority.High => 10,
                ReminderPriority.Medium => 15,
                ReminderPriority.Normal => 30,
                ReminderPriority.Low => 60,
                _ => 15
            };
        }

        public List<DateTime> CalculateReminderTimes(
            DateTime eventDateTime,
            List<int> minutesBeforeList)
        {
            var reminderTimes = new List<DateTime>();

            foreach (var minutesBefore in minutesBeforeList.OrderBy(m => m))
            {
                var reminderTime = eventDateTime.AddMinutes(-minutesBefore);
                
                // Only add if in the future
                if (reminderTime > Clock.Now)
                {
                    reminderTimes.Add(reminderTime);
                }
            }

            return reminderTimes;
        }

        public bool ShouldBypassDoNotDisturb(Reminder reminder)
        {
            return reminder.Priority == ReminderPriority.Critical;
        }

        public string GetSoundPattern(ReminderPriority priority)
        {
            return priority switch
            {
                ReminderPriority.Critical => "critical",
                ReminderPriority.High => "urgent",
                ReminderPriority.Medium => "triple",
                ReminderPriority.Normal => "double",
                ReminderPriority.Low => "single",
                _ => "single"
            };
        }
    }
}
