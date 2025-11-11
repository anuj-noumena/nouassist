using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Habits.Dtos
{
    public class HabitTrackingDto : CreationAuditedEntityDto<Guid>
    {
        public Guid HabitId { get; set; }
        public DateTime LogDate { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Notes { get; set; }
    }

    public class CompleteHabitDto
    {
        public DateTime LogDate { get; set; }
        public string Notes { get; set; }
    }

    public class HabitMetricsDto
    {
        public Guid HabitId { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public double CompletionRate { get; set; }
        public double Last7DaysRate { get; set; }
        public double Last30DaysRate { get; set; }
        public int ConsistencyScore { get; set; }
        public bool IsStrong { get; set; }
        public int TotalCompletions { get; set; }
        public int TotalAttempts { get; set; }
        public int? BestDayOfWeek { get; set; }
    }
}
