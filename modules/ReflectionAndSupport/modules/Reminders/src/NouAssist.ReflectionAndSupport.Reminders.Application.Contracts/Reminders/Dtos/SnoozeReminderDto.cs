using System.ComponentModel.DataAnnotations;

namespace NouAssist.ReflectionAndSupport.Reminders.Reminders.Dtos
{
    public class SnoozeReminderDto
    {
        [Required]
        [Range(1, 1440)] // 1 minute to 24 hours
        public int Minutes { get; set; }
    }
}
