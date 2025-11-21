using System;
using System.Linq.Expressions;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Specifications
{
    /// <summary>
    /// Base specification class for Portfolio aggregate
    /// </summary>
    public abstract class PortfolioSpecification : Specification<PortfolioAggregate>
    {
    }

    /// <summary>
    /// Specification that identifies active portfolios
    /// </summary>
    public class ActivePortfolioSpecification : PortfolioSpecification
    {
        public override Expression<Func<PortfolioAggregate, bool>> ToExpression()
        {
            return portfolio => portfolio.IsActive;
        }
    }

    /// <summary>
    /// Specification that identifies portfolios with high risk
    /// </summary>
    public class HighRiskPortfolioSpecification : PortfolioSpecification
    {
        private readonly decimal _riskScoreThreshold;

        public HighRiskPortfolioSpecification(decimal riskScoreThreshold = 6.0m)
        {
            _riskScoreThreshold = riskScoreThreshold;
        }

        public override Expression<Func<PortfolioAggregate, bool>> ToExpression()
        {
            // Note: This is a limitation as we can't easily express the calculated risk score
            // in an Expression. In real world, we might need a separate risk score stored field
            // or implement this filtering in-memory after fetching portfolios.
            return portfolio => portfolio.Risks.Any(r => 
                r.Assessment.Level == RiskLevel.High || 
                r.Assessment.Level == RiskLevel.Critical);
        }
    }

    /// <summary>
    /// Specification that identifies portfolios containing a specific item
    /// </summary>
    public class PortfolioContainingItemSpecification : PortfolioSpecification
    {
        private readonly Guid _itemId;
        private readonly PortfolioItemType _itemType;

        public PortfolioContainingItemSpecification(Guid itemId, PortfolioItemType itemType)
        {
            _itemId = itemId;
            _itemType = itemType;
        }

        public override Expression<Func<PortfolioAggregate, bool>> ToExpression()
        {
            return portfolio => portfolio.Items.Any(i => 
                i.Reference.ItemId == _itemId && i.Reference.ItemType == _itemType);
        }
    }
}
