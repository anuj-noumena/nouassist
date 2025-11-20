using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.FinancialPlanning.Specifications
{
    public class ActiveGoalsSpecification : Specification<FinancialGoal>
    {
        public override Expression<Func<FinancialGoal, bool>> ToExpression() =>
            g => g.Status == GoalStatus.Active;
    }

    public class GoalsByUserSpecification : Specification<FinancialGoal>
    {
        private readonly Guid _userId;

        public GoalsByUserSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<FinancialGoal, bool>> ToExpression() =>
            g => g.UserId == _userId;
    }

    public class GoalsByTypeSpecification : Specification<FinancialGoal>
    {
        private readonly GoalType _type;

        public GoalsByTypeSpecification(GoalType type)
        {
            _type = type;
        }

        public override Expression<Func<FinancialGoal, bool>> ToExpression() =>
            g => g.Type == _type;
    }

    public class GoalsDueSoonSpecification : Specification<FinancialGoal>
    {
        private readonly int _daysAhead;

        public GoalsDueSoonSpecification(int daysAhead = 30)
        {
            _daysAhead = daysAhead;
        }

        public override Expression<Func<FinancialGoal, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(_daysAhead);
            return g => g.Status == GoalStatus.Active && g.TargetDate <= cutoffDate;
        }
    }

    public class CompletedGoalsSpecification : Specification<FinancialGoal>
    {
        public override Expression<Func<FinancialGoal, bool>> ToExpression() =>
            g => g.Status == GoalStatus.Achieved;
    }
}
