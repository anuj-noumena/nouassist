using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Interfaces.Services
{
    /// <summary>
    /// Service interface for feature prioritization logic and algorithms
    /// </summary>
    public interface IFeaturePrioritizationEngine
    {
        /// <summary>
        /// Calculates priority scores for multiple features using ICE methodology
        /// </summary>
        Task<List<FeaturePriorityResult>> CalculateIcePriorityAsync(
            List<FeatureDefinition> features);

        /// <summary>
        /// Generates feature prioritization matrix
        /// </summary>
        Task<PrioritizationMatrix> GeneratePrioritizationMatrixAsync(
            List<FeatureDefinition> features,
            PrioritizationCriteria criteria);

        /// <summary>
        /// Recommends MVP feature set based on prioritization
        /// </summary>
        Task<MvpRecommendation> RecommendMvpFeaturesAsync(
            List<FeatureDefinition> features,
            MvpConstraints constraints);

        /// <summary>
        /// Performs feature dependency analysis
        /// </summary>
        Task<FeatureDependencyAnalysis> AnalyzeFeatureDependenciesAsync(
            List<FeatureDefinition> features);

        /// <summary>
        /// Generates prioritization recommendations
        /// </summary>
        Task<List<PrioritizationRecommendation>> GenerateRecommendationsAsync(
            List<FeatureDefinition> features);
    }

    /// <summary>
    /// Data transfer object for feature priority results
    /// </summary>
    public class FeaturePriorityResult
    {
        public Guid FeatureId { get; set; }
        public string FeatureName { get; set; }
        public decimal IceScore { get; set; }
        public FeaturePriority RecommendedPriority { get; set; }
        public string Rationale { get; set; }
        public Dictionary<string, decimal> CriteriaScores { get; set; }
        public int Rank { get; set; }
    }

    /// <summary>
    /// Data transfer object for prioritization criteria
    /// </summary>
    public class PrioritizationCriteria
    {
        public decimal ImpactWeight { get; set; } = 1.0m;
        public decimal ConfidenceWeight { get; set; } = 1.0m;
        public decimal EffortWeight { get; set; } = 1.0m;
        public Dictionary<string, decimal> CustomCriteria { get; set; }
        public bool IncludeCustomerFeedback { get; set; }
        public bool IncludeBusinessValue { get; set; }
    }

    /// <summary>
    /// Data transfer object for prioritization matrix
    /// </summary>
    public class PrioritizationMatrix
    {
        public List<List<FeaturePriorityResult>> Matrix { get; set; }
        public List<string> XAxisLabels { get; set; }
        public List<string> YAxisLabels { get; set; }
        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
        public Dictionary<string, List<FeaturePriorityResult>> Quadrants { get; set; }
    }

    /// <summary>
    /// Data transfer object for MVP constraints
    /// </summary>
    public class MvpConstraints
    {
        public int MaxFeatureCount { get; set; }
        public decimal MaxDevelopmentEffort { get; set; }
        public DateTime TargetLaunchDate { get; set; }
        public List<Guid> MustHaveFeatures { get; set; }
        public List<Guid> ExcludedFeatures { get; set; }
        public decimal MinimumIceScore { get; set; }
    }

    /// <summary>
    /// Data transfer object for MVP recommendations
    /// </summary>
    public class MvpRecommendation
    {
        public List<FeaturePriorityResult> RecommendedFeatures { get; set; }
        public List<FeaturePriorityResult> AlternativeFeatures { get; set; }
        public decimal TotalEffortEstimate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public string Rationale { get; set; }
        public List<string> Risks { get; set; }
        public List<string> Assumptions { get; set; }
        public decimal ConfidenceLevel { get; set; }
    }

    /// <summary>
    /// Data transfer object for feature dependency analysis
    /// </summary>
    public class FeatureDependencyAnalysis
    {
        public Dictionary<Guid, List<Guid>> Dependencies { get; set; }
        public List<FeatureDependencyChain> DependencyChains { get; set; }
        public List<Guid> CoreFeatures { get; set; }
        public List<Guid> OptionalFeatures { get; set; }
        public List<string> CircularDependencies { get; set; }
        public Dictionary<Guid, int> DependencyLevels { get; set; }
    }

    /// <summary>
    /// Data transfer object for feature dependency chains
    /// </summary>
    public class FeatureDependencyChain
    {
        public List<Guid> FeatureIds { get; set; }
        public List<string> FeatureNames { get; set; }
        public int ChainLength { get; set; }
        public bool IsCriticalPath { get; set; }
    }

    /// <summary>
    /// Data transfer object for prioritization recommendations
    /// </summary>
    public class PrioritizationRecommendation
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public List<Guid> AffectedFeatures { get; set; }
        public string Impact { get; set; }
        public FeaturePriority Priority { get; set; }
        public List<string> ActionItems { get; set; }
    }
}
