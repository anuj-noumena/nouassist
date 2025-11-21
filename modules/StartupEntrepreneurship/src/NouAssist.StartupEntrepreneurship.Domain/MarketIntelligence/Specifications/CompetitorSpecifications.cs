using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Specifications
{
    /// <summary>
    /// Base specification for competitors
    /// </summary>
    public abstract class CompetitorSpecification : Specification<Competitor>
    {
    }

    /// <summary>
    /// Specification for active competitors
    /// </summary>
    public class ActiveCompetitorSpecification : CompetitorSpecification
    {
        public override Expression<Func<Competitor, bool>> ToExpression()
        {
            return c => c.Status == CompetitorStatus.Active;
        }
    }

    /// <summary>
    /// Specification for high-threat competitors
    /// </summary>
    public class HighThreatCompetitorSpecification : CompetitorSpecification
    {
        public override Expression<Func<Competitor, bool>> ToExpression()
        {
            return c => c.ThreatLevel == CompetitorThreatLevel.High || 
                        c.ThreatLevel == CompetitorThreatLevel.Critical;
        }
    }

    /// <summary>
    /// Specification for competitors in a specific industry
    /// </summary>
    public class CompetitorInIndustrySpecification : CompetitorSpecification
    {
        private readonly string _industry;

        public CompetitorInIndustrySpecification(string industry)
        {
            _industry = industry;
        }

        public override Expression<Func<Competitor, bool>> ToExpression()
        {
            return c => c.CompetitorInfo.Industry.Contains(_industry);
        }
    }

    /// <summary>
    /// Specification for competitors with recent updates
    /// </summary>
    public class RecentlyUpdatedCompetitorSpecification : CompetitorSpecification
    {
        private readonly int _days;

        public RecentlyUpdatedCompetitorSpecification(int days = 30)
        {
            _days = days;
        }

        public override Expression<Func<Competitor, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_days);
            return c => c.UpdateLogs.Any(u => u.UpdateDate >= cutoffDate);
        }
    }

    /// <summary>
    /// Specification for competitors with high competitiveness score
    /// </summary>
    public class HighlyCompetitiveCompetitorSpecification : CompetitorSpecification
    {
        private readonly decimal _minimumScore;

        public HighlyCompetitiveCompetitorSpecification(decimal minimumScore = 0.7m)
        {
            _minimumScore = minimumScore;
        }

        public override Expression<Func<Competitor, bool>> ToExpression()
        {
            return c => c.CompetitiveScore >= _minimumScore;
        }
    }
}
