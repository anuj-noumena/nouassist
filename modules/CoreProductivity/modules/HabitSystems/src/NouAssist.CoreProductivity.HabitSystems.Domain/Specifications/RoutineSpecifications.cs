using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.CoreProductivity.HabitSystems.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.CoreProductivity.HabitSystems.Specifications
{
    public class ActiveRoutinesSpecification : Specification<Routine>
    {
        public override Expression<Func<Routine, bool>> ToExpression() =>
            r => r.IsActive;
    }

    public class RoutinesWithStepsSpecification : Specification<Routine>
    {
        public override Expression<Func<Routine, bool>> ToExpression() =>
            r => r.Steps.Any();
    }

    public class ScheduledRoutinesSpecification : Specification<Routine>
    {
        public override Expression<Func<Routine, bool>> ToExpression() =>
            r => r.ScheduledDays.Any();
    }

    public class RoutinesByTimeOfDaySpecification : Specification<Routine>
    {
        private readonly string _timeOfDay;

        public RoutinesByTimeOfDaySpecification(string timeOfDay)
        {
            _timeOfDay = timeOfDay;
        }

        public override Expression<Func<Routine, bool>> ToExpression() =>
            r => r.TimeOfDay == _timeOfDay;
    }
}
