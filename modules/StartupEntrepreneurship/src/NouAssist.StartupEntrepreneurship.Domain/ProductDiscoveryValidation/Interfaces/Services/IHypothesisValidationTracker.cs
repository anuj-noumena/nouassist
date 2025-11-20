namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Interfaces.Services
{
    /// <summary>
    /// Service interface for tracking and analyzing hypothesis validation progress
    /// </summary>
    public interface IHypothesisValidationTracker
    {
        /// <summary>
        /// Calculates overall validation progress for a hypothesis
        /// </summary>
        Task<ValidationProgress> CalculateValidationProgressAsync(
            ProblemHypothesis hypothesis);

        /// <summary>
        /// Identifies validation gaps and recommendations
        /// </summary>
        Task<List<ValidationRecommendation>> GetValidationRecommendationsAsync(
            ProblemHypothesis hypothesis);

        /// <summary>
        /// Tracks validation metrics across multiple hypotheses
        /// </summary>
        Task<ValidationPortfolioMetrics> AnalyzeValidationPortfolioAsync(
            List<ProblemHypothesis> hypotheses);

        /// <summary>
        /// Predicts validation outcome based on current progress
        /// </summary>
        Task<ValidationOutcomePrediction> PredictValidationOutcomeAsync(
            ProblemHypothesis hypothesis);

        /// <summary>
        /// Generates validation status report
        /// </summary>
        Task<ValidationStatusReport> GenerateStatusReportAsync(
            List<ProblemHypothesis> hypotheses);
    }

    /// <summary>
    /// Data transfer object for validation progress
    /// </summary>
    public class ValidationProgress
    {
        public Guid HypothesisId { get; set; }
        public decimal CompletionPercentage { get; set; }
        public int PlannedAttempts { get; set; }
        public int CompletedAttempts { get; set; }
        public ValidationStatus CurrentStatus { get; set; }
        public List<string> CompletedMethods { get; set; }
        public List<string> PendingMethods { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }
    }

    /// <summary>
    /// Data transfer object for validation recommendations
    /// </summary>
    public class ValidationRecommendation
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public FeaturePriority Priority { get; set; }
        public string Rationale { get; set; }
        public List<string> SuggestedMethods { get; set; }
        public int EstimatedEffortDays { get; set; }
    }

    /// <summary>
    /// Data transfer object for validation portfolio metrics
    /// </summary>
    public class ValidationPortfolioMetrics
    {
        public int TotalHypotheses { get; set; }
        public Dictionary<ValidationStatus, int> StatusDistribution { get; set; }
        public decimal OverallCompletionRate { get; set; }
        public decimal ValidationSuccessRate { get; set; }
        public List<string> CommonValidationMethods { get; set; }
        public int AverageDaysToValidation { get; set; }
        public List<ValidationTrend> Trends { get; set; }
    }

    /// <summary>
    /// Data transfer object for validation outcome prediction
    /// </summary>
    public class ValidationOutcomePrediction
    {
        public ValidationStatus PredictedOutcome { get; set; }
        public decimal Confidence { get; set; }
        public string Reasoning { get; set; }
        public List<string> RiskFactors { get; set; }
        public List<string> SuccessFactors { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
    }

    /// <summary>
    /// Data transfer object for validation status report
    /// </summary>
    public class ValidationStatusReport
    {
        public DateTime GeneratedDate { get; set; }
        public ValidationPortfolioMetrics PortfolioMetrics { get; set; }
        public List<ValidationProgress> HypothesesProgress { get; set; }
        public List<ValidationRecommendation> TopRecommendations { get; set; }
        public List<string> KeyFindings { get; set; }
        public List<string> ActionItems { get; set; }
    }

    /// <summary>
    /// Data transfer object for validation trends
    /// </summary>
    public class ValidationTrend
    {
        public string Metric { get; set; }
        public string Period { get; set; }
        public string Direction { get; set; }
        public decimal PercentageChange { get; set; }
        public string Description { get; set; }
    }
}
