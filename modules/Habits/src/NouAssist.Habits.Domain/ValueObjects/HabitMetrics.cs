using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Habits.ValueObjects
{
    public class HabitMetrics : ValueObject
    {
        public Guid HabitId { get; init; }
        public int CurrentStreak { get; init; }
        public int LongestStreak { get; init; }
        public double CompletionRate { get; init; }
        public double Last7DaysRate { get; init; }
        public double Last30DaysRate { get; init; }
        public int ConsistencyScore { get; init; }
        public bool IsStrong { get; init; }
        public int TotalCompletions { get; init; }
        public int TotalAttempts { get; init; }
        public int? BestDayOfWeek { get; init; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return HabitId;
            yield return CurrentStreak;
            yield return LongestStreak;
            yield return CompletionRate;
            yield return Last7DaysRate;
            yield return Last30DaysRate;
            yield return ConsistencyScore;
            yield return TotalCompletions;
            yield return TotalAttempts;
        }
    }
}
