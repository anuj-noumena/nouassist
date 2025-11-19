using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.FinancialPlanning.Specifications
{
    public class ExpensesByUserSpecification : Specification<Expense>
    {
        private readonly Guid _userId;

        public ExpensesByUserSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<Expense, bool>> ToExpression() =>
            e => e.UserId == _userId;
    }

    public class ExpensesByCategorySpecification : Specification<Expense>
    {
        private readonly Guid _categoryId;

        public ExpensesByCategorySpecification(Guid categoryId)
        {
            _categoryId = categoryId;
        }

        public override Expression<Func<Expense, bool>> ToExpression() =>
            e => e.CategoryId == _categoryId;
    }

    public class ExpensesInPeriodSpecification : Specification<Expense>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public ExpensesInPeriodSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<Expense, bool>> ToExpression() =>
            e => e.ExpenseDate >= _startDate && e.ExpenseDate <= _endDate;
    }

    public class UnreportedExpensesSpecification : Specification<Expense>
    {
        public override Expression<Func<Expense, bool>> ToExpression() =>
            e => e.Status == ExpenseStatus.Approved && e.ExpenseReportId == null;
    }

    public class PendingExpensesSpecification : Specification<Expense>
    {
        public override Expression<Func<Expense, bool>> ToExpression() =>
            e => e.Status == ExpenseStatus.Pending;
    }

    public class LargeExpensesSpecification : Specification<Expense>
    {
        private readonly decimal _threshold;

        public LargeExpensesSpecification(decimal threshold)
        {
            _threshold = threshold;
        }

        public override Expression<Func<Expense, bool>> ToExpression() =>
            e => e.Amount.Amount >= _threshold;
    }
}
