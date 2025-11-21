using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Specifications
{
    /// <summary>
    /// Base specification for SWOT analyses
    /// </summary>
    public abstract class SwotAnalysisSpecification : Specification<SwotAnalysis>
    {
    }

    /// <summary>
    /// Specification for completed SWOT analyses
    /// </summary>
    public class CompletedSwotAnalysisSpecification : SwotAnalysisSpecification
    {
        public override Expression<Func<SwotAnalysis, bool>> ToExpression()
        {
            return s => s.IsCompleted;
        }
    }

    /// <summary>
    /// Specification for SWOT analyses created within a specific timeframe
    /// </summary>
    public class RecentSwotAnalysisSpecification : SwotAnalysisSpecification
    {
        private readonly int _days;

        public RecentSwotAnalysisSpecification(int days = 90)
        {
            _days = days;
        }

        public override Expression<Func<SwotAnalysis, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_days);
            return s => s.AnalysisDate >= cutoffDate;
        }
    }

    /// <summary>
    /// Specification for SWOT analyses with balanced coverage across categories
    /// </summary>
    public class BalancedSwotAnalysisSpecification : SwotAnalysisSpecification
    {
        public override Expression<Func<SwotAnalysis, bool>> ToExpression()
        {
            return s => s.Items.GroupBy(i => i.Category).Count() == 4 && // Has all 4 categories
                        s.Items.GroupBy(i => i.Category).All(g => g.Count() >= 2); // At least 2 items per category
        }
    }

    /// <summary>
    /// Specification for SWOT analyses with high-priority items
    /// </summary>
    public class HighPriorityItemsSwotAnalysisSpecification : SwotAnalysisSpecification
    {
        public override Expression<Func<SwotAnalysis, bool>> ToExpression()
        {
            return s => s.Items.Any(i => i.Priority <= 3 && i.Impact >= 0.7m);
        }
    }
}
