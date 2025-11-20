using System;
using System.Linq.Expressions;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Specifications
{
    /// <summary>
    /// Base specification for recommendations
    /// </summary>
    public abstract class RecommendationSpecification : Specification<RecommendationInstance>
    {
    }

    /// <summary>
    /// Specification for active recommendations
    /// </summary>
    public class ActiveRecommendationSpecification : RecommendationSpecification
    {
        public override Expression<Func<RecommendationInstance, bool>> ToExpression()
        {
            return r => !r.IsActedOn && !r.IsDismissed && 
                        (!r.ExpiresAt.HasValue || r.ExpiresAt.Value > DateTime.UtcNow);
        }
    }

    /// <summary>
    /// Specification for high priority recommendations
    /// </summary>
    public class HighPriorityRecommendationSpecification : RecommendationSpecification
    {
        private readonly int _minimumPriorityScore;

        public HighPriorityRecommendationSpecification(int minimumPriorityScore = 80)
        {
            _minimumPriorityScore = minimumPriorityScore;
        }

        public override Expression<Func<RecommendationInstance, bool>> ToExpression()
        {
            return r => r.Priority.Value >= _minimumPriorityScore;
        }
    }

    /// <summary>
    /// Specification for recommendations of a specific type
    /// </summary>
    public class RecommendationTypeSpecification : RecommendationSpecification
    {
        private readonly RecommendationType _recommendationType;

        public RecommendationTypeSpecification(RecommendationType recommendationType)
        {
            _recommendationType = recommendationType;
        }

        public override Expression<Func<RecommendationInstance, bool>> ToExpression()
        {
            return r => r.RecommendationType == _recommendationType;
        }
    }

    /// <summary>
    /// Specification for recommendations that need attention
    /// </summary>
    public class AttentionNeededRecommendationSpecification : RecommendationSpecification
    {
        public override Expression<Func<RecommendationInstance, bool>> ToExpression()
        {
            var currentTime = DateTime.UtcNow;
            return r => !r.IsActedOn && !r.IsDismissed && 
                        r.Priority.Value >= 70 &&
                        r.GeneratedAt.AddDays(1) < currentTime &&
                        (!r.ExpiresAt.HasValue || r.ExpiresAt.Value > currentTime);
        }
    }
}
