namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories
{
    /// <summary>
    /// Repository interface for CustomerSegment entity
    /// </summary>
    public interface ICustomerSegmentRepository : IRepository<CustomerSegment, Guid>
    {
        /// <summary>
        /// Finds segments matching the given specification
        /// </summary>
        Task<List<CustomerSegment>> GetListAsync(
            Expression<Func<CustomerSegment, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds completed segments (target met)
        /// </summary>
        Task<List<CustomerSegment>> GetCompletedSegmentsAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds segments with low completion rates
        /// </summary>
        Task<List<CustomerSegment>> GetLowCompletionSegmentsAsync(
            decimal maximumCompletionPercentage = 50m,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds recently created segments
        /// </summary>
        Task<List<CustomerSegment>> GetRecentSegmentsAsync(
            int daysAgo = 30,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets segment completion summary
        /// </summary>
        Task<SegmentCompletionSummary> GetCompletionSummaryAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches segments by name or description
        /// </summary>
        Task<List<CustomerSegment>> SearchAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets segment statistics
        /// </summary>
        Task<SegmentStatistics> GetStatisticsAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets segments that need attention
        /// </summary>
        Task<List<CustomerSegment>> GetNeedingAttentionAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds segments by specific criteria
        /// </summary>
        Task<List<CustomerSegment>> FindByAttributesAsync(
            Dictionary<string, object> attributes,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Data transfer object for segment completion summary
    /// </summary>
    public class SegmentCompletionSummary
    {
        public int TotalSegments { get; set; }
        public int CompletedSegments { get; set; }
        public int InProgressSegments { get; set; }
        public int NotStartedSegments { get; set; }
        public decimal OverallCompletionRate { get; set; }
        public int TotalTargetSampleSize { get; set; }
        public int TotalActualSampleSize { get; set; }
        public List<string> SegmentsNeedingAttention { get; set; }
    }

    /// <summary>
    /// Data transfer object for segment statistics
    /// </summary>
    public class SegmentStatistics
    {
        public int TotalSegments { get; set; }
        public decimal AverageTargetSampleSize { get; set; }
        public decimal AverageActualSampleSize { get; set; }
        public decimal AverageCompletionPercentage { get; set; }
        public Dictionary<string, int> SegmentsByCompletion { get; set; }
        public List<string> TopPerformingSegments { get; set; }
        public List<string> UnderperformingSegments { get; set; }
        public int SegmentsNeedingAttention { get; set; }
    }
}
