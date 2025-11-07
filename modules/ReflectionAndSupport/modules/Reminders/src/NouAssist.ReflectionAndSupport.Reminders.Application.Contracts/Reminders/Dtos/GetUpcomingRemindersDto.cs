using System.ComponentModel.DataAnnotations;

namespace NouAssist.ReflectionAndSupport.Reminders.Reminders.Dtos
{
    public class GetUpcomingRemindersDto
    {
        [Range(1, 168)] // 1 hour to 1 week
        public int HoursAhead { get; set; } = 24;
    }
}
