using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects
{
    /// <summary>
    /// Value object representing impact assessment for trends or opportunities
    /// </summary>
    public class ImpactAssessment : ValueObject
    {
        public ImpactLevel Level { get; private set; }
        public string Description { get; private set; }
        public decimal ConfidenceScore { get; private set; }
        public string RecommendedActions { get; private set; }

        private ImpactAssessment() { }

        public ImpactAssessment(
            ImpactLevel level,
            string description,
            decimal confidenceScore,
            string recommendedActions = null)
        {
            Level = level;
            SetDescription(description);
            SetConfidenceScore(confidenceScore);
            RecommendedActions = recommendedActions?.Trim();
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: MarketIntelligenceConsts.MarketTrend.MaxImpactDescriptionLength
            );
        }

        private void SetConfidenceScore(decimal confidenceScore)
        {
            if (confidenceScore < MarketIntelligenceConsts.Validation.MinConfidenceScore ||
                confidenceScore > MarketIntelligenceConsts.Validation.MaxConfidenceScore)
            {
                throw new BusinessException("MarketIntelligence:Validation:InvalidScore")
                    .WithData("min", MarketIntelligenceConsts.Validation.MinConfidenceScore)
                    .WithData("max", MarketIntelligenceConsts.Validation.MaxConfidenceScore);
            }

            ConfidenceScore = confidenceScore;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Level;
            yield return Description;
            yield return ConfidenceScore;
            yield return RecommendedActions;
        }
    }
}
