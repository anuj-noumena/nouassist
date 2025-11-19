using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.FinancialPlanning.Specifications
{
    public class ActiveBudgetsSpecification : Specification<Budget>
    {
        public override Expression<Func<Budget, bool>> ToExpression() =>
            b => b.Status == BudgetStatus.Active;
    }

    public class BudgetsByUserSpecification : Specification<Budget>
    {
        private readonly Guid _userId;

        public BudgetsByUserSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<Budget, bool>> ToExpression() =>
            b => b.UserId == _userId;
    }

    public class CurrentBudgetsSpecification : Specification<Budget>
    {
        private readonly DateTime _date;

        public CurrentBudgetsSpecification(DateTime? date = null)
        {
            _date = date ?? DateTime.UtcNow;
        }

        public override Expression<Func<Budget, bool>> ToExpression() =>
            b => b.PeriodStart <= _date && b.PeriodEnd >= _date;
    }

    public class BudgetsByTypeSpecification : Specification<Budget>
    {
        private readonly BudgetType _type;

        public BudgetsByTypeSpecification(BudgetType type)
        {
            _type = type;
        }

        public override Expression<Func<Budget, bool>> ToExpression() =>
            b => b.Type == _type;
    }
}
