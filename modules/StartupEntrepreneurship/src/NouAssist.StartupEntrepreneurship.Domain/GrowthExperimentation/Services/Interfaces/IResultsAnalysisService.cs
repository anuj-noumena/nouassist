namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services.Interfaces
{
    /// <summary>
    /// Service for analyzing experiment results
    /// </summary>
    public interface IResultsAnalysisService
    {
        /// <summary>
        /// Records result data for an experiment variant
        /// </summary>
        Task RecordResultDataAsync(
            Guid variantId,
            string metricName,
            MetricType metricType,
            double value,
            int sampleSize,
            double? standardDeviation = null,
            double? marginOfError = null,
            bool isPrimary = false);
            
        /// <summary>
        /// Analyzes results for statistical significance
        /// </summary>
        Task<StatisticalSignificance> AnalyzeSignificanceAsync(
            Guid experimentId,
            string primaryMetricName,
            double confidenceLevel = GrowthExperimentationConstants.Experiments.DefaultSignificanceLevel);
            
        /// <summary>
        /// Determines the winning variant based on results
        /// </summary>
        Task<Guid?> DetermineWinningVariantAsync(
            Guid experimentId,
            string primaryMetricName);
            
        /// <summary>
        /// Completes an experiment with final analysis
        /// </summary>
        Task CompleteExperimentAnalysisAsync(
            Guid experimentId,
            ExperimentLearnings learnings);
    }
}
