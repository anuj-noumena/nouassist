using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Specifications
{
    /// <summary>
    /// Base specification class for InvestorProfile aggregate
    /// </summary>
    public abstract class InvestorSpecification : Specification<InvestorProfile>
    {
    }

    /// <summary>
    /// Specification for active investors with recent activity
    /// </summary>
    public class ActiveInvestorSpecification : InvestorSpecification
    {
        private readonly TimeSpan _activityPeriod;

        public ActiveInvestorSpecification(TimeSpan? activityPeriod = null)
        {
            _activityPeriod = activityPeriod ?? TimeSpan.FromDays(90);
        }

        public override Expression<Func<InvestorProfile, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.Subtract(_activityPeriod);
            return investor => investor.Status == InvestorStatus.Active &&
                              investor.LastInteractionDate >= cutoffDate;
        }
    }

    /// <summary>
    /// Specification for investors by type and minimum investment
    /// </summary>
    public class InvestorByTypeAndInvestmentSpecification : InvestorSpecification
    {
        private readonly InvestorType _investorType;
        private readonly decimal _minimumInvestment;

        public InvestorByTypeAndInvestmentSpecification(InvestorType investorType, decimal minimumInvestment)
        {
            _investorType = investorType;
            _minimumInvestment = minimumInvestment;
        }

        public override Expression<Func<InvestorProfile, bool>> ToExpression()
        {
            return investor => investor.Type == _investorType &&
                              investor.Investments.Where(i => i.IsActive)
                                     .Sum(i => i.Commitment.InvestedAmount) >= _minimumInvestment;
        }
    }

    /// <summary>
    /// Specification for high-value investors with strong relationships
    /// </summary>
    public class HighValueInvestorSpecification : InvestorSpecification
    {
        private readonly decimal _minimumInvestment;
        private readonly int _minimumRelationshipScore;

        public HighValueInvestorSpecification(decimal minimumInvestment = 100000, int minimumRelationshipScore = 80)
        {
            _minimumInvestment = minimumInvestment;
            _minimumRelationshipScore = minimumRelationshipScore;
        }

        public override Expression<Func<InvestorProfile, bool>> ToExpression()
        {
            return investor => investor.Status == InvestorStatus.Active &&
                              investor.RelationshipScore.Score >= _minimumRelationshipScore &&
                              investor.Investments.Where(i => i.IsActive)
                                     .Sum(i => i.Commitment.InvestedAmount) >= _minimumInvestment;
        }
    }
}
