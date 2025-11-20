using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.Notification.Reminders.Enums;

namespace NouAssist.Notification.Reminders.Reminders.Dtos
{
    public class UpdateReminderDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public DateTime RemindAt { get; set; }

        public ReminderPriority Priority { get; set; }
        public string Notes { get; set; }
    }
}
