using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Reminders.Reminders.Dtos
{
    public class ReminderDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public Guid OwnerId { get; set; }
        public string RefEntityType { get; set; }
        public Guid? RefEntityId { get; set; }
        public DateTime RemindAt { get; set; }
        public string Priority { get; set; }
        public string Method { get; set; }
        public bool IsSent { get; set; }
        public DateTime? SentAt { get; set; }
        public string Status { get; set; }
        public ReminderScheduleDto Schedule { get; set; }
        public SnoozeInfoDto SnoozeInfo { get; set; }
        public int SnoozeCount { get; set; }
        public string Notes { get; set; }
        
        // Computed properties
        public bool IsPending { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsDue { get; set; }
        public bool IsRecurring { get; set; }
        public int? HoursUntilReminder { get; set; }
    }

    public class ReminderScheduleDto
    {
        public DateTime BaseDateTime { get; set; }
        public int MinutesBeforeEvent { get; set; }
        public string RecurrencePattern { get; set; }
        public int? RecurrenceInterval { get; set; }
        public DateTime? RecurrenceEndDate { get; set; }
        public string[] RecurrenceDaysOfWeek { get; set; }
    }

    public class SnoozeInfoDto
    {
        public DateTime OriginalRemindAt { get; set; }
        public DateTime NewRemindAt { get; set; }
        public int SnoozeDurationMinutes { get; set; }
        public int SnoozeCount { get; set; }
        public DateTime SnoozedAt { get; set; }
    }
}
