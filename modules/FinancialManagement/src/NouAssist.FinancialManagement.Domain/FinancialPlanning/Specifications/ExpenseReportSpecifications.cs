using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.FinancialPlanning.Specifications
{
    public class PendingApprovalReportsSpecification : Specification<ExpenseReport>
    {
        public override Expression<Func<ExpenseReport, bool>> ToExpression() =>
            r => r.Status == ReportStatus.Submitted;
    }

    public class ReportsBySubmitterSpecification : Specification<ExpenseReport>
    {
        private readonly Guid _submitterId;

        public ReportsBySubmitterSpecification(Guid submitterId)
        {
            _submitterId = submitterId;
        }

        public override Expression<Func<ExpenseReport, bool>> ToExpression() =>
            r => r.SubmitterId == _submitterId;
    }

    public class DraftReportsSpecification : Specification<ExpenseReport>
    {
        public override Expression<Func<ExpenseReport, bool>> ToExpression() =>
            r => r.Status == ReportStatus.Draft;
    }

    public class ApprovedReportsSpecification : Specification<ExpenseReport>
    {
        public override Expression<Func<ExpenseReport, bool>> ToExpression() =>
            r => r.Status == ReportStatus.Approved;
    }

    public class ReportsInPeriodSpecification : Specification<ExpenseReport>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public ReportsInPeriodSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<ExpenseReport, bool>> ToExpression() =>
            r => r.PeriodStart >= _startDate && r.PeriodEnd <= _endDate;
    }
}
