namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Specifications
{
    /// <summary>
    /// Base specification class for FeatureDefinition aggregate
    /// </summary>
    public abstract class FeatureDefinitionSpecification : Specification<FeatureDefinition>
    {
    }

    /// <summary>
    /// Specification for MVP-ready features
    /// </summary>
    public class MvpReadyFeatureSpecification : FeatureDefinitionSpecification
    {
        public override Expression<Func<FeatureDefinition, bool>> ToExpression()
        {
            return feature => 
                feature.Priority >= FeaturePriority.High &&
                feature.MvpStatus == MvpStatus.Planned &&
                feature.PriorityScores.Any();
        }
    }

    /// <summary>
    /// Specification for high-impact features
    /// </summary>
    public class HighImpactFeatureSpecification : FeatureDefinitionSpecification
    {
        private readonly decimal _minimumPriorityScore;

        public HighImpactFeatureSpecification(decimal minimumPriorityScore = 10m)
        {
            _minimumPriorityScore = minimumPriorityScore;
        }

        public override Expression<Func<FeatureDefinition, bool>> ToExpression()
        {
            return feature => 
                feature.PriorityScores.Any() &&
                feature.PriorityScores.Average(s => s.CalculatedScore) >= _minimumPriorityScore;
        }
    }

    /// <summary>
    /// Specification for features related to validated hypotheses
    /// </summary>
    public class ValidatedHypothesisFeatureSpecification : FeatureDefinitionSpecification
    {
        public override Expression<Func<FeatureDefinition, bool>> ToExpression()
        {
            return feature => 
                feature.RelatedHypothesisId.HasValue;
        }
    }

    /// <summary>
    /// Specification for features needing re-prioritization
    /// </summary>
    public class NeedsPrioritizationSpecification : FeatureDefinitionSpecification
    {
        private readonly int _daysSinceLastPrioritization;

        public NeedsPrioritizationSpecification(int daysSinceLastPrioritization = 30)
        {
            _daysSinceLastPrioritization = daysSinceLastPrioritization;
        }

        public override Expression<Func<FeatureDefinition, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_daysSinceLastPrioritization);
            return feature => 
                !feature.PriorityScores.Any() ||
                feature.PriorityScores.All(s => s.CalculatedAt <= cutoffDate);
        }
    }

    /// <summary>
    /// Specification for features with prototype validation
    /// </summary>
    public class PrototypeValidatedFeatureSpecification : FeatureDefinitionSpecification
    {
        public override Expression<Func<FeatureDefinition, bool>> ToExpression()
        {
            return feature => 
                feature.RelatedPrototypeId.HasValue;
        }
    }
}
