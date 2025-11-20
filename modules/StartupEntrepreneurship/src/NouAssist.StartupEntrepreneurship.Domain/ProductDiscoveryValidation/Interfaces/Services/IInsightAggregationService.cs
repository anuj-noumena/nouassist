namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Interfaces.Services
{
    /// <summary>
    /// Service interface for aggregating and analyzing insights from customer interviews
    /// </summary>
    public interface IInsightAggregationService
    {
        /// <summary>
        /// Aggregates insights by category across multiple interviews
        /// </summary>
        Task<Dictionary<string, List<Insight>>> AggregateInsightsByCategoryAsync(
            List<CustomerInterview> interviews);

        /// <summary>
        /// Identifies common themes across insights
        /// </summary>
        Task<List<InsightTheme>> IdentifyCommonThemesAsync(
            List<CustomerInterview> interviews,
            int minimumOccurrences = 2);

        /// <summary>
        /// Calculates insight distribution metrics
        /// </summary>
        Task<InsightDistributionMetrics> CalculateDistributionMetricsAsync(
            List<CustomerInterview> interviews);

        /// <summary>
        /// Generates insight summary for a customer segment
        /// </summary>
        Task<SegmentInsightSummary> GenerateSegmentSummaryAsync(
            Guid customerSegmentId);
    }

    /// <summary>
    /// Data transfer object for insight themes
    /// </summary>
    public class InsightTheme
    {
        public string Theme { get; set; }
        public int Occurrences { get; set; }
        public decimal AverageImportance { get; set; }
        public List<Guid> RelatedInterviewIds { get; set; }
        public List<string> KeyQuotes { get; set; }
    }

    /// <summary>
    /// Data transfer object for insight distribution metrics
    /// </summary>
    public class InsightDistributionMetrics
    {
        public int TotalInsights { get; set; }
        public decimal AverageImportance { get; set; }
        public Dictionary<string, int> CategoryDistribution { get; set; }
        public Dictionary<int, int> ImportanceDistribution { get; set; }
        public List<string> TopCategories { get; set; }
    }

    /// <summary>
    /// Data transfer object for segment insight summary
    /// </summary>
    public class SegmentInsightSummary
    {
        public Guid SegmentId { get; set; }
        public string SegmentName { get; set; }
        public int InterviewCount { get; set; }
        public int TotalInsights { get; set; }
        public List<InsightTheme> TopThemes { get; set; }
        public List<string> KeyPainPoints { get; set; }
        public List<string> IdentifiedOpportunities { get; set; }
        public decimal OverallSentiment { get; set; }
    }
}
