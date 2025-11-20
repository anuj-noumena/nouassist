namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Interfaces.Services
{
    /// <summary>
    /// Service interface for analyzing user feedback from testing sessions
    /// </summary>
    public interface IFeedbackAnalysisService
    {
        /// <summary>
        /// Analyzes feedback patterns across sessions
        /// </summary>
        Task<FeedbackPatternAnalysis> AnalyzeFeedbackPatternsAsync(
            List<UserFeedbackSession> sessions);

        /// <summary>
        /// Calculates satisfaction metrics
        /// </summary>
        Task<SatisfactionMetrics> CalculateSatisfactionMetricsAsync(
            List<UserFeedbackSession> sessions);

        /// <summary>
        /// Identifies improvement opportunities from feedback
        /// </summary>
        Task<List<ImprovementOpportunity>> IdentifyImprovementOpportunitiesAsync(
            List<UserFeedbackSession> sessions);

        /// <summary>
        /// Generates feedback summary for a prototype
        /// </summary>
        Task<PrototypeFeedbackSummary> GeneratePrototypeFeedbackSummaryAsync(
            Guid prototypeId);

        /// <summary>
        /// Performs sentiment analysis on feedback comments
        /// </summary>
        Task<SentimentAnalysisResult> AnalyzeFeedbackSentimentAsync(
            List<UserFeedbackSession> sessions);
    }

    /// <summary>
    /// Data transfer object for feedback pattern analysis
    /// </summary>
    public class FeedbackPatternAnalysis
    {
        public Dictionary<string, List<FeedbackPattern>> PatternsByCategory { get; set; }
        public List<string> RecurringIssues { get; set; }
        public List<string> ConsistentPositives { get; set; }
        public Dictionary<string, decimal> CategorySatisfactionScores { get; set; }
        public List<FeedbackCorrelation> Correlations { get; set; }
    }

    /// <summary>
    /// Data transfer object for feedback patterns
    /// </summary>
    public class FeedbackPattern
    {
        public string Pattern { get; set; }
        public int Frequency { get; set; }
        public decimal AverageRating { get; set; }
        public List<string> Examples { get; set; }
        public string Impact { get; set; }
    }

    /// <summary>
    /// Data transfer object for feedback correlations
    /// </summary>
    public class FeedbackCorrelation
    {
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public decimal CorrelationCoefficient { get; set; }
        public string Interpretation { get; set; }
    }

    /// <summary>
    /// Data transfer object for satisfaction metrics
    /// </summary>
    public class SatisfactionMetrics
    {
        public decimal OverallSatisfaction { get; set; }
        public Dictionary<string, decimal> CategorySatisfaction { get; set; }
        public Dictionary<string, int> ResponseDistribution { get; set; }
        public decimal NetPromoterScore { get; set; }
        public List<string> TopPositives { get; set; }
        public List<string> TopConcerns { get; set; }
        public int TotalResponses { get; set; }
    }

    /// <summary>
    /// Data transfer object for improvement opportunities
    /// </summary>
    public class ImprovementOpportunity
    {
        public string Area { get; set; }
        public string Description { get; set; }
        public decimal Impact { get; set; }
        public string Evidence { get; set; }
        public List<string> SuggestedActions { get; set; }
        public FeaturePriority Priority { get; set; }
        public int AffectedUsers { get; set; }
    }

    /// <summary>
    /// Data transfer object for prototype feedback summary
    /// </summary>
    public class PrototypeFeedbackSummary
    {
        public Guid PrototypeId { get; set; }
        public string PrototypeName { get; set; }
        public int SessionCount { get; set; }
        public int ParticipantCount { get; set; }
        public SatisfactionMetrics Satisfaction { get; set; }
        public List<ImprovementOpportunity> TopOpportunities { get; set; }
        public List<string> KeyInsights { get; set; }
        public Dictionary<string, string> VersionComparisons { get; set; }
        public string OverallAssessment { get; set; }
    }

    /// <summary>
    /// Data transfer object for sentiment analysis results
    /// </summary>
    public class SentimentAnalysisResult
    {
        public decimal OverallSentiment { get; set; }
        public Dictionary<string, decimal> CategorySentiment { get; set; }
        public List<string> PositiveFeedback { get; set; }
        public List<string> NegativeFeedback { get; set; }
        public List<string> NeutralFeedback { get; set; }
        public Dictionary<string, int> EmotionDistribution { get; set; }
        public List<SentimentTrend> Trends { get; set; }
    }

    /// <summary>
    /// Data transfer object for sentiment trends
    /// </summary>
    public class SentimentTrend
    {
        public DateTime Date { get; set; }
        public decimal SentimentScore { get; set; }
        public int ResponseCount { get; set; }
        public string Category { get; set; }
    }
}
