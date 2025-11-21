using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Specifications
{
    /// <summary>
    /// Base specification for market trends
    /// </summary>
    public abstract class MarketTrendSpecification : Specification<MarketTrend>
    {
    }

    /// <summary>
    /// Specification for active market trends
    /// </summary>
    public class ActiveMarketTrendSpecification : MarketTrendSpecification
    {
        public override Expression<Func<MarketTrend, bool>> ToExpression()
        {
            return t => t.IsActive && (!t.ExpiryDate.HasValue || t.ExpiryDate.Value > DateTime.UtcNow);
        }
    }

    /// <summary>
    /// Specification for high-impact market trends
    /// </summary>
    public class HighImpactMarketTrendSpecification : MarketTrendSpecification
    {
        public override Expression<Func<MarketTrend, bool>> ToExpression()
        {
            return t => t.ImpactAssessment.Level == ImpactLevel.High || 
                        t.ImpactAssessment.Level == ImpactLevel.Critical;
        }
    }

    /// <summary>
    /// Specification for market trends with high confidence
    /// </summary>
    public class HighConfidenceMarketTrendSpecification : MarketTrendSpecification
    {
        private readonly decimal _minimumConfidence;

        public HighConfidenceMarketTrendSpecification(decimal minimumConfidence = 0.7m)
        {
            _minimumConfidence = minimumConfidence;
        }

        public override Expression<Func<MarketTrend, bool>> ToExpression()
        {
            return t => t.ImpactAssessment.ConfidenceScore >= _minimumConfidence;
        }
    }

    /// <summary>
    /// Specification for market trends in specific category
    /// </summary>
    public class TrendInCategorySpecification : MarketTrendSpecification
    {
        private readonly TrendCategory _category;

        public TrendInCategorySpecification(TrendCategory category)
        {
            _category = category;
        }

        public override Expression<Func<MarketTrend, bool>> ToExpression()
        {
            return t => t.Category == _category;
        }
    }

    /// <summary>
    /// Specification for market trends containing specific keywords
    /// </summary>
    public class TrendWithKeywordsSpecification : MarketTrendSpecification
    {
        private readonly string _keywords;

        public TrendWithKeywordsSpecification(string keywords)
        {
            _keywords = keywords?.ToLower() ?? string.Empty;
        }

        public override Expression<Func<MarketTrend, bool>> ToExpression()
        {
            return t => (t.TrendDescription.Keywords != null && t.TrendDescription.Keywords.ToLower().Contains(_keywords)) ||
                        t.TrendDescription.Title.ToLower().Contains(_keywords) ||
                        t.TrendDescription.Description.ToLower().Contains(_keywords);
        }
    }
}
