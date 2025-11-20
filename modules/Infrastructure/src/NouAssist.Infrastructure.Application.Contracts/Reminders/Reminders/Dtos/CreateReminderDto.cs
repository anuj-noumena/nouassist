using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.Notification.Reminders.Enums;

namespace NouAssist.Notification.Reminders.Reminders.Dtos
{
    public class CreateReminderDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public DateTime RemindAt { get; set; }

        public ReminderEntityType? RefEntityType { get; set; }
        public Guid? RefEntityId { get; set; }
        public ReminderPriority Priority { get; set; } = ReminderPriority.Normal;
        public string Method { get; set; } = "Push";
        public string Notes { get; set; }
    }
}
