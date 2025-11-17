using System;
using System.Linq.Expressions;
using NouAssist.CreditScoreMonitoring.Enums;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.Specifications
{
    public class LatestCreditReportsSpecification : Specification<CreditReport>
    {
        private readonly int _daysThreshold;

        public LatestCreditReportsSpecification(int daysThreshold = 90)
        {
            _daysThreshold = daysThreshold;
        }

        public override Expression<Func<CreditReport, bool>> ToExpression() =>
            r => r.ReportDate >= DateTime.UtcNow.AddDays(-_daysThreshold);
    }

    public class PoorCreditScoreSpecification : Specification<CreditReport>
    {
        public override Expression<Func<CreditReport, bool>> ToExpression() =>
            r => r.ScoreRange == CreditScoreRange.Poor || r.ScoreRange == CreditScoreRange.Fair;
    }

    public class HighUtilizationSpecification : Specification<CreditReport>
    {
        private readonly decimal _threshold;

        public HighUtilizationSpecification(decimal utilizationThreshold = 50)
        {
            _threshold = utilizationThreshold;
        }

        public override Expression<Func<CreditReport, bool>> ToExpression() =>
            r => r.CreditUtilizationPercentage >= _threshold;
    }
}
