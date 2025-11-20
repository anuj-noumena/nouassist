using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.Notification.Reminders.Enums;

namespace NouAssist.Notification.Reminders.Reminders.Dtos
{
    public class CreateRecurringReminderDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public DateTime BaseDateTime { get; set; }

        [Required]
        public int MinutesBeforeEvent { get; set; }

        [Required]
        public RecurrencePattern RecurrencePattern { get; set; }

        public int? RecurrenceInterval { get; set; }
        public DateTime? RecurrenceEndDate { get; set; }
        public List<DayOfWeek> RecurrenceDaysOfWeek { get; set; }

        public ReminderEntityType? RefEntityType { get; set; }
        public Guid? RefEntityId { get; set; }
        public ReminderPriority Priority { get; set; } = ReminderPriority.Normal;
        public string Method { get; set; } = "Push";
        public string Notes { get; set; }
    }
}
