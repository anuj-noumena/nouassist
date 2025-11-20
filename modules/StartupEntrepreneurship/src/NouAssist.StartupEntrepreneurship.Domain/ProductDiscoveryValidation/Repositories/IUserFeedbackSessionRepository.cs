namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories
{
    /// <summary>
    /// Repository interface for UserFeedbackSession aggregate
    /// </summary>
    public interface IUserFeedbackSessionRepository : IRepository<UserFeedbackSession, Guid>
    {
        /// <summary>
        /// Finds sessions matching the given specification
        /// </summary>
        Task<List<UserFeedbackSession>> GetListAsync(
            Expression<Func<UserFeedbackSession, bool>> predicate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds sessions by status
        /// </summary>
        Task<List<UserFeedbackSession>> GetByStatusAsync(
            InterviewStatus status,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds sessions related to a prototype
        /// </summary>
        Task<List<UserFeedbackSession>> GetByPrototypeAsync(
            Guid prototypeId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets session with all feedback data
        /// </summary>
        Task<UserFeedbackSession> GetWithFeedbackAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds sessions with comprehensive feedback
        /// </summary>
        Task<List<UserFeedbackSession>> GetWithComprehensiveFeedbackAsync(
            int minimumFeedbackCount = 5,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds sessions needing analysis
        /// </summary>
        Task<List<UserFeedbackSession>> GetNeedingAnalysisAsync(
            int daysSinceCompletion = 7,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets feedback session statistics
        /// </summary>
        Task<FeedbackSessionStatistics> GetStatisticsAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches sessions by participant or content
        /// </summary>
        Task<List<UserFeedbackSession>> SearchAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets sessions for satisfaction analysis
        /// </summary>
        Task<List<UserFeedbackSession>> GetForSatisfactionAnalysisAsync(
            Guid? prototypeId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets average ratings by category
        /// </summary>
        Task<Dictionary<string, decimal>> GetAverageRatingsByCategoryAsync(
            Guid? prototypeId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts sessions by prototype
        /// </summary>
        Task<Dictionary<string, int>> GetSessionCountByPrototypeAsync(
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Data transfer object for feedback session statistics
    /// </summary>
    public class FeedbackSessionStatistics
    {
        public int TotalSessions { get; set; }
        public int CompletedSessions { get; set; }
        public int ScheduledSessions { get; set; }
        public int CancelledSessions { get; set; }
        public int SessionsWithComprehensiveFeedback { get; set; }
        public decimal AverageFeedbackPerSession { get; set; }
        public Dictionary<string, decimal> AverageRatingsByCategory { get; set; }
        public Dictionary<string, int> SessionsByPrototype { get; set; }
        public int SessionsNeedingAnalysis { get; set; }
    }
}
