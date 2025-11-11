using System;
using System.Linq.Expressions;
using NouAssist.Habits.Aggregates;
using NouAssist.Habits.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Habits.Specifications
{
    public class HealthEntriesWithGoodSleepSpecification : Specification<HealthTrackingEntry>
    {
        public override Expression<Func<HealthTrackingEntry, bool>> ToExpression() =>
            e => e.SleepHours >= 7 && e.SleepHours <= 9;
    }

    public class HealthEntriesWithAdequateWaterSpecification : Specification<HealthTrackingEntry>
    {
        public override Expression<Func<HealthTrackingEntry, bool>> ToExpression() =>
            e => e.WaterCount >= 8;
    }

    public class HighEnergyEntriesSpecification : Specification<EnergyTrackingEntry>
    {
        public override Expression<Func<EnergyTrackingEntry, bool>> ToExpression() =>
            e => e.EnergyLevel >= 8;
    }

    public class LowEnergyEntriesSpecification : Specification<EnergyTrackingEntry>
    {
        public override Expression<Func<EnergyTrackingEntry, bool>> ToExpression() =>
            e => e.EnergyLevel <= 3;
    }

    public class PositiveMoodEntriesSpecification : Specification<MoodTrackingEntry>
    {
        public override Expression<Func<MoodTrackingEntry, bool>> ToExpression() =>
            e => e.MoodRating >= 7;
    }

    public class NegativeMoodEntriesSpecification : Specification<MoodTrackingEntry>
    {
        public override Expression<Func<MoodTrackingEntry, bool>> ToExpression() =>
            e => e.MoodRating <= 4;
    }

    public class ProductiveFocusSessionsSpecification : Specification<FocusSession>
    {
        private readonly int _minimumPomodoros;

        public ProductiveFocusSessionsSpecification(int minimumPomodoros = 4)
        {
            _minimumPomodoros = minimumPomodoros;
        }

        public override Expression<Func<FocusSession, bool>> ToExpression() =>
            s => s.CompletedPomodoros >= _minimumPomodoros;
    }

    public class CompletedReflectionsSpecification : Specification<DailyReflection>
    {
        public override Expression<Func<DailyReflection, bool>> ToExpression() =>
            r => r.IsComplete;
    }

    public class CompletedRoutineCompletionsSpecification : Specification<RoutineCompletion>
    {
        public override Expression<Func<RoutineCompletion, bool>> ToExpression() =>
            c => c.Status == RoutineCompletionStatus.Completed;
    }

    public class PartialRoutineCompletionsSpecification : Specification<RoutineCompletion>
    {
        public override Expression<Func<RoutineCompletion, bool>> ToExpression() =>
            c => c.Status == RoutineCompletionStatus.PartiallyCompleted;
    }
}
