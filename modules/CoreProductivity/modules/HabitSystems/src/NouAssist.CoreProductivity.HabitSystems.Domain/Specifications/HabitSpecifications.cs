using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.CoreProductivity.HabitSystems.Aggregates;
using NouAssist.CoreProductivity.HabitSystems.Enums;
using Volo.Abp.Specifications;
using DayOfWeek = NouAssist.CoreProductivity.HabitSystems.Enums.DayOfWeek;

namespace NouAssist.CoreProductivity.HabitSystems.Specifications
{
    public class ActiveHabitsSpecification : Specification<Habit>
    {
        public override Expression<Func<Habit, bool>> ToExpression() =>
            h => h.IsActive;
    }

    public class HabitsByTypeSpecification : Specification<Habit>
    {
        private readonly HabitType _habitType;

        public HabitsByTypeSpecification(HabitType habitType)
        {
            _habitType = habitType;
        }

        public override Expression<Func<Habit, bool>> ToExpression() =>
            h => h.HabitType == _habitType;
    }

    public class HabitsByAreaSpecification : Specification<Habit>
    {
        private readonly Guid _areaId;

        public HabitsByAreaSpecification(Guid areaId)
        {
            _areaId = areaId;
        }

        public override Expression<Func<Habit, bool>> ToExpression() =>
            h => h.AreaId == _areaId;
    }

    public class HabitsScheduledForDaySpecification : Specification<Habit>
    {
        private readonly DayOfWeek _dayOfWeek;

        public HabitsScheduledForDaySpecification(DateTime date)
        {
            _dayOfWeek = (DayOfWeek)((int)date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek);
        }

        public override Expression<Func<Habit, bool>> ToExpression() =>
            h => !h.ScheduledDays.Any() || h.ScheduledDays.Contains(_dayOfWeek);
    }

    public class StrongHabitsSpecification : Specification<Habit>
    {
        public override Expression<Func<Habit, bool>> ToExpression() =>
            h => h.IsActive; // Will be combined with tracking data in domain service
    }
}
