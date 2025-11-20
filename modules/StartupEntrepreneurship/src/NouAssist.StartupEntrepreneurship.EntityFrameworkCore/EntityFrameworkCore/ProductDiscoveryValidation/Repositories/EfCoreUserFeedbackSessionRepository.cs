namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of UserFeedbackSession repository
    /// </summary>
    public class EfCoreUserFeedbackSessionRepository 
        : EfCoreRepository<IProductDiscoveryDbContext, UserFeedbackSession, Guid>, IUserFeedbackSessionRepository
    {
        public EfCoreUserFeedbackSessionRepository(
            IDbContextProvider<IProductDiscoveryDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<UserFeedbackSession>> GetListAsync(
            Expression<Func<UserFeedbackSession, bool>> predicate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(s => s.FeedbackData);
            }

            return await query
                .Where(predicate)
                .OrderByDescending(s => s.ScheduledDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserFeedbackSession>> GetByStatusAsync(
            InterviewStatus status,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                s => s.Status == status,
                includeDetails,
                cancellationToken);
        }

        public async Task<List<UserFeedbackSession>> GetByPrototypeAsync(
            Guid prototypeId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                s => s.RelatedPrototypeId == prototypeId,
                includeDetails,
                cancellationToken);
        }

        public async Task<UserFeedbackSession> GetWithFeedbackAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            var session = await query
                .Include(s => s.FeedbackData)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if (session == null)
                throw new EntityNotFoundException(typeof(UserFeedbackSession), id);

            return session;
        }

        public async Task<List<UserFeedbackSession>> GetWithComprehensiveFeedbackAsync(
            int minimumFeedbackCount = 5,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Include(s => s.FeedbackData)
                .Where(s => s.Status == InterviewStatus.Completed &&
                           s.FeedbackData.Count >= minimumFeedbackCount)
                .OrderByDescending(s => s.FeedbackData.Count)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserFeedbackSession>> GetNeedingAnalysisAsync(
            int daysSinceCompletion = 7,
            CancellationToken cancellationToken = default)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysSinceCompletion);

            return await GetListAsync(
                s => s.Status == InterviewStatus.Completed &&
                     s.CompletedDate.HasValue &&
                     s.CompletedDate.Value <= cutoffDate,
                true,
                cancellationToken);
        }

        public async Task<FeedbackSessionStatistics> GetStatisticsAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (fromDate.HasValue)
                query = query.Where(s => s.ScheduledDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(s => s.ScheduledDate <= toDate.Value);

            var sessions = await query
                .Include(s => s.FeedbackData)
                .ToListAsync(cancellationToken);

            var totalSessions = sessions.Count;
            var completedSessions = sessions.Count(s => s.Status == InterviewStatus.Completed);
            var scheduledSessions = sessions.Count(s => s.Status == InterviewStatus.Scheduled);
            var cancelledSessions = sessions.Count(s => s.Status == InterviewStatus.Cancelled);

            var sessionsWithComprehensiveFeedback = sessions.Count(s => 
                s.Status == InterviewStatus.Completed && s.FeedbackData.Count >= 5);

            var allFeedback = sessions.SelectMany(s => s.FeedbackData).ToList();
            var avgFeedbackPerSession = completedSessions > 0 
                ? (decimal)allFeedback.Count / completedSessions 
                : 0;

            var avgRatingsByCategory = allFeedback
                .GroupBy(f => f.Category)
                .ToDictionary(g => g.Key, g => g.Average(f => f.Rating.PercentageScore()));

            var sessionsByPrototype = sessions
                .Where(s => s.RelatedPrototypeId.HasValue)
                .GroupBy(s => s.RelatedPrototypeId.Value)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            var sessionsNeedingAnalysis = sessions.Count(s => 
                s.Status == InterviewStatus.Completed &&
                s.CompletedDate.HasValue &&
                s.CompletedDate.Value <= DateTime.UtcNow.AddDays(-7));

            return new FeedbackSessionStatistics
            {
                TotalSessions = totalSessions,
                CompletedSessions = completedSessions,
                ScheduledSessions = scheduledSessions,
                CancelledSessions = cancelledSessions,
                SessionsWithComprehensiveFeedback = sessionsWithComprehensiveFeedback,
                AverageFeedbackPerSession = avgFeedbackPerSession,
                AverageRatingsByCategory = avgRatingsByCategory,
                SessionsByPrototype = sessionsByPrototype,
                SessionsNeedingAnalysis = sessionsNeedingAnalysis
            };
        }

        public async Task<List<UserFeedbackSession>> SearchAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<UserFeedbackSession>();

            var query = await GetQueryableAsync();
            searchTerm = searchTerm.ToLower();

            return await query
                .Where(s => s.Title.ToLower().Contains(searchTerm) ||
                           s.Description.ToLower().Contains(searchTerm) ||
                           s.ParticipantEmail.ToLower().Contains(searchTerm) ||
                           s.TestScenario.ToLower().Contains(searchTerm))
                .OrderByDescending(s => s.ScheduledDate)
                .Take(maxResults)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserFeedbackSession>> GetForSatisfactionAnalysisAsync(
            Guid? prototypeId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query.Where(s => s.Status == InterviewStatus.Completed);

            if (prototypeId.HasValue)
                query = query.Where(s => s.RelatedPrototypeId == prototypeId.Value);

            if (fromDate.HasValue)
                query = query.Where(s => s.ScheduledDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(s => s.ScheduledDate <= toDate.Value);

            return await query
                .Include(s => s.FeedbackData)
                .OrderByDescending(s => s.CompletedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<string, decimal>> GetAverageRatingsByCategoryAsync(
            Guid? prototypeId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            var sessions = await GetForSatisfactionAnalysisAsync(prototypeId, fromDate, toDate, cancellationToken);
            var allFeedback = sessions.SelectMany(s => s.FeedbackData).ToList();

            if (!allFeedback.Any())
                return new Dictionary<string, decimal>();

            return allFeedback
                .GroupBy(f => f.Category)
                .ToDictionary(g => g.Key, g => g.Average(f => f.Rating.PercentageScore()));
        }

        public async Task<Dictionary<string, int>> GetSessionCountByPrototypeAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(s => s.RelatedPrototypeId.HasValue)
                .GroupBy(s => s.RelatedPrototypeId.Value)
                .ToDictionaryAsync(g => g.Key.ToString(), g => g.Count(), cancellationToken);
        }

        public override async Task<IQueryable<UserFeedbackSession>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(s => s.FeedbackData);
        }
    }
}
