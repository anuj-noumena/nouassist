using System;
using NouAssist.Notification.Reminders.Enums;
using NouAssist.Notification.Reminders.Events;
using NouAssist.Notification.Reminders.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Notification.Reminders.Aggregates
{
    public class Reminder : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public const int MaxTitleLength = 200;

        public string Title { get; private set; }
        public Guid OwnerId { get; private set; }
        public ReminderEntityType? RefEntityType { get; private set; }
        public Guid? RefEntityId { get; private set; }
        public DateTime RemindAt { get; private set; }
        public ReminderPriority Priority { get; private set; }
        public string Method { get; private set; } // Notification channel as string
        public bool IsSent { get; private set; }
        public DateTime? SentAt { get; private set; }
        public ReminderStatus Status { get; private set; }
        public ReminderSchedule Schedule { get; private set; }
        public SnoozeInfo SnoozeInfo { get; private set; }
        public int SnoozeCount { get; private set; }
        public string Notes { get; private set; }
        public Guid? TenantId { get; private set; }

        protected Reminder() { /* ORM */ }

        public Reminder(
            Guid id,
            Guid ownerId,
            string title,
            DateTime remindAt,
            Guid? tenantId,
            ReminderEntityType? refEntityType = null,
            Guid? refEntityId = null,
            ReminderPriority priority = ReminderPriority.Normal,
            string method = "Push") : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), MaxTitleLength);
            RemindAt = remindAt;
            TenantId = tenantId;
            RefEntityType = refEntityType;
            RefEntityId = refEntityId;
            Priority = priority;
            Method = method ?? "Push";
            
            IsSent = false;
            Status = ReminderStatus.Pending;
            SnoozeCount = 0;
        }

        public void SetSchedule(ReminderSchedule schedule)
        {
            Schedule = Check.NotNull(schedule, nameof(schedule));
            RemindAt = schedule.CalculateReminderTime();
        }

        public void Snooze(int minutes)
        {
            if (IsSent)
            {
                throw new BusinessException("Reminders:CannotSnooze", "Cannot snooze a reminder that has already been sent.");
            }

            if (minutes <= 0)
            {
                throw new BusinessException("Reminders:InvalidSnoozeDuration", "Snooze duration must be positive.");
            }

            var originalRemindAt = RemindAt;
            var newRemindAt = DateTime.UtcNow.AddMinutes(minutes);
            
            SnoozeInfo = new SnoozeInfo(
                originalRemindAt,
                newRemindAt,
                minutes,
                SnoozeCount + 1,
                DateTime.UtcNow
            );

            RemindAt = newRemindAt;
            Status = ReminderStatus.Snoozed;
            SnoozeCount++;

            // Publish snooze event
            AddDistributedEvent(new ReminderSnoozedEto
            {
                ReminderId = Id,
                OwnerId = OwnerId,
                OriginalRemindAt = originalRemindAt,
                NewRemindAt = newRemindAt,
                SnoozeDurationMinutes = minutes,
                TenantId = TenantId
            });
        }

        public void Reschedule(DateTime newRemindAt)
        {
            if (IsSent)
            {
                throw new BusinessException("Reminders:CannotReschedule", "Cannot reschedule a sent reminder.");
            }

            RemindAt = Check.NotNull(newRemindAt, nameof(newRemindAt));
            Status = ReminderStatus.Pending;
            
            // Clear snooze info when explicitly rescheduled
            SnoozeInfo = null;
        }

        public void MarkAsSent()
        {
            if (IsSent) return;

            IsSent = true;
            SentAt = DateTime.UtcNow;
            Status = ReminderStatus.Sent;

            // Publish triggered event
            AddDistributedEvent(new ReminderTriggeredEto
            {
                ReminderId = Id,
                OwnerId = OwnerId,
                Title = Title,
                EntityType = RefEntityType?.ToString(),
                EntityId = RefEntityId,
                Priority = Priority.ToString(),
                RemindAt = RemindAt,
                TriggeredAt = SentAt.Value,
                TenantId = TenantId
            });

            // If recurring, create next occurrence
            if (Schedule?.IsRecurring == true)
            {
                var nextOccurrence = Schedule.CalculateNextOccurrence(RemindAt);
                if (nextOccurrence.HasValue)
                {
                    RemindAt = nextOccurrence.Value;
                    IsSent = false;
                    SentAt = null;
                    Status = ReminderStatus.Pending;
                    SnoozeCount = 0;
                    SnoozeInfo = null;
                }
            }
        }

        public void Cancel()
        {
            Status = ReminderStatus.Cancelled;
        }

        public void UpdatePriority(ReminderPriority priority)
        {
            Priority = priority;
        }

        public void UpdateTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), MaxTitleLength);
        }

        public void SetNotes(string notes)
        {
            Notes = notes;
        }

        public bool HasReference => RefEntityType.HasValue && RefEntityId.HasValue;
        public bool IsPending => Status == ReminderStatus.Pending && !IsSent && DateTime.UtcNow < RemindAt;
        public bool IsOverdue => !IsSent && DateTime.UtcNow > RemindAt && Status != ReminderStatus.Cancelled;
        public bool IsDue => !IsSent && DateTime.UtcNow >= RemindAt && Status == ReminderStatus.Pending;
        public bool IsSnoozed => Status == ReminderStatus.Snoozed;
        public bool IsCancelled => Status == ReminderStatus.Cancelled;
        public bool IsRecurring => Schedule?.IsRecurring == true;
        public bool WasSnoozed => SnoozeCount > 0;

        public TimeSpan? GetTimeUntilReminder()
        {
            if (IsSent || IsCancelled) return null;
            
            var timeUntil = RemindAt - DateTime.UtcNow;
            return timeUntil.TotalSeconds > 0 ? timeUntil : TimeSpan.Zero;
        }

        public int? GetHoursUntilReminder()
        {
            var timeUntil = GetTimeUntilReminder();
            return timeUntil?.Hours;
        }

        public int? GetMinutesUntilReminder()
        {
            var timeUntil = GetTimeUntilReminder();
            return (int?)timeUntil?.TotalMinutes;
        }
    }
}
