using System;
using System.Linq.Expressions;
using NouAssist.PersonalDevelopment.Goals.Aggregates;
using NouAssist.PersonalDevelopment.Goals.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.PersonalDevelopment.Goals.Specifications
{
    public class ActiveGoalsSpecification : Specification<Goal>
    {
        public override Expression<Func<Goal, bool>> ToExpression() =>
            g => g.Status == GoalStatus.Active;
    }

    public class OverdueGoalsSpecification : Specification<Goal>
    {
        public override Expression<Func<Goal, bool>> ToExpression() =>
            g => g.TargetDate.HasValue && 
                 g.TargetDate.Value < DateTime.UtcNow && 
                 g.Status == GoalStatus.Active &&
                 g.Progress < 100;
    }

    public class GoalsByAreaSpecification : Specification<Goal>
    {
        private readonly Guid _areaId;

        public GoalsByAreaSpecification(Guid areaId)
        {
            _areaId = areaId;
        }

        public override Expression<Func<Goal, bool>> ToExpression() =>
            g => g.AreaId == _areaId;
    }

    public class HighPriorityGoalsSpecification : Specification<Goal>
    {
        public override Expression<Func<Goal, bool>> ToExpression() =>
            g => g.Importance.Value >= 0.75 && g.Status == GoalStatus.Active;
    }

    public class NearlyCompleteGoalsSpecification : Specification<Goal>
    {
        public override Expression<Func<Goal, bool>> ToExpression() =>
            g => g.Progress >= 75 && 
                 g.Progress < 100 && 
                 g.Status == GoalStatus.Active;
    }

    public class GoalsCompletedInMonthSpecification : Specification<Goal>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public GoalsCompletedInMonthSpecification(int year, int month)
        {
            _startDate = new DateTime(year, month, 1);
            _endDate = _startDate.AddMonths(1);
        }

        public override Expression<Func<Goal, bool>> ToExpression() =>
            g => g.Status == GoalStatus.Achieved &&
                 g.CompletedAt.HasValue &&
                 g.CompletedAt.Value >= _startDate &&
                 g.CompletedAt.Value < _endDate;
    }
}
