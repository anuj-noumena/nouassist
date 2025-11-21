using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Interfaces.Services
{
    /// <summary>
    /// Service interface for calculating problem-solution fit metrics
    /// </summary>
    public interface IProblemSolutionFitCalculator
    {
        /// <summary>
        /// Calculates problem-solution fit score
        /// </summary>
        Task<ProblemSolutionFitScore> CalculateFitScoreAsync(
            ProblemHypothesis hypothesis,
            List<UserFeedbackSession> feedbackSessions);

        /// <summary>
        /// Analyzes fit across multiple hypotheses
        /// </summary>
        Task<FitAnalysisReport> AnalyzeFitAcrossHypothesesAsync(
            List<ProblemHypothesis> hypotheses);

        /// <summary>
        /// Tracks fit evolution over time
        /// </summary>
        Task<FitEvolutionAnalysis> AnalyzeFitEvolutionAsync(
            ProblemHypothesis hypothesis);

        /// <summary>
        /// Generates fit improvement recommendations
        /// </summary>
        Task<List<FitImprovementRecommendation>> GenerateFitRecommendationsAsync(
            ProblemHypothesis hypothesis);

        /// <summary>
        /// Benchmarks fit against industry standards
        /// </summary>
        Task<FitBenchmarkResult> BenchmarkFitAsync(
            ProblemSolutionFitScore fitScore,
            string industry = null);
    }

    /// <summary>
    /// Data transfer object for problem-solution fit score
    /// </summary>
    public class ProblemSolutionFitScore
    {
        public Guid HypothesisId { get; set; }
        public decimal OverallScore { get; set; }
        public ProblemSolutionFitMetric DetailedMetrics { get; set; }
        public string FitLevel { get; set; }
        public Dictionary<string, decimal> ComponentScores { get; set; }
        public List<string> StrengthAreas { get; set; }
        public List<string> WeaknessAreas { get; set; }
        public decimal Confidence { get; set; }
        public DateTime CalculatedDate { get; set; }
    }

    /// <summary>
    /// Data transfer object for fit analysis report
    /// </summary>
    public class FitAnalysisReport
    {
        public List<ProblemSolutionFitScore> HypothesesScores { get; set; }
        public decimal AverageScore { get; set; }
        public ProblemSolutionFitScore BestPerforming { get; set; }
        public ProblemSolutionFitScore WorstPerforming { get; set; }
        public Dictionary<string, int> FitLevelDistribution { get; set; }
        public List<string> PortfolioInsights { get; set; }
        public List<string> RecommendedActions { get; set; }
    }

    /// <summary>
    /// Data transfer object for fit evolution analysis
    /// </summary>
    public class FitEvolutionAnalysis
    {
        public Guid HypothesisId { get; set; }
        public List<FitEvolutionPoint> EvolutionPoints { get; set; }
        public string Trend { get; set; }
        public decimal TrendStrength { get; set; }
        public List<string> InfluencingFactors { get; set; }
        public List<FitMilestone> Milestones { get; set; }
        public FitProjection Projection { get; set; }
    }

    /// <summary>
    /// Data transfer object for fit evolution points
    /// </summary>
    public class FitEvolutionPoint
    {
        public DateTime Date { get; set; }
        public decimal Score { get; set; }
        public string Event { get; set; }
        public string Notes { get; set; }
    }

    /// <summary>
    /// Data transfer object for fit milestones
    /// </summary>
    public class FitMilestone
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal ScoreChange { get; set; }
        public string Impact { get; set; }
    }

    /// <summary>
    /// Data transfer object for fit projections
    /// </summary>
    public class FitProjection
    {
        public DateTime ProjectionDate { get; set; }
        public decimal ProjectedScore { get; set; }
        public decimal Confidence { get; set; }
        public List<string> Assumptions { get; set; }
        public List<string> Scenarios { get; set; }
    }

    /// <summary>
    /// Data transfer object for fit improvement recommendations
    /// </summary>
    public class FitImprovementRecommendation
    {
        public string Area { get; set; }
        public string Recommendation { get; set; }
        public decimal PotentialImprovement { get; set; }
        public FeaturePriority Priority { get; set; }
        public int ImplementationEffort { get; set; }
        public List<string> ActionSteps { get; set; }
        public string Success Criteria { get; set; }
    }

    /// <summary>
    /// Data transfer object for fit benchmark results
    /// </summary>
    public class FitBenchmarkResult
    {
        public decimal YourScore { get; set; }
        public decimal IndustryAverage { get; set; }
        public decimal TopQuartile { get; set; }
        public decimal BottomQuartile { get; set; }
        public string Percentile { get; set; }
        public string Interpretation { get; set; }
        public List<string> BenchmarkInsights { get; set; }
        public List<string> ImprovementOpportunities { get; set; }
    }
}
