using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using NouAssist.FinancialManagement.InvestmentManagement.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.InvestmentManagement.Specifications
{
    public class ActivePortfoliosSpecification : Specification<Portfolio>
    {
        public override Expression<Func<Portfolio, bool>> ToExpression() =>
            p => p.Status == PortfolioStatus.Active;
    }

    public class PortfoliosNeedingRebalanceSpecification : Specification<Portfolio>
    {
        private readonly int _daysThreshold;

        public PortfoliosNeedingRebalanceSpecification(int daysThreshold = 90)
        {
            _daysThreshold = daysThreshold;
        }

        public override Expression<Func<Portfolio, bool>> ToExpression() =>
            p => p.Status == PortfolioStatus.Active &&
                 (!p.LastRebalanceDate.HasValue || 
                  p.LastRebalanceDate.Value < DateTime.UtcNow.AddDays(-_daysThreshold));
    }

    public class HighValuePortfoliosSpecification : Specification<Portfolio>
    {
        private readonly decimal _valueThreshold;

        public HighValuePortfoliosSpecification(decimal valueThreshold = 100000m)
        {
            _valueThreshold = valueThreshold;
        }

        public override Expression<Func<Portfolio, bool>> ToExpression() =>
            p => p.TotalValue >= _valueThreshold;
    }
}
