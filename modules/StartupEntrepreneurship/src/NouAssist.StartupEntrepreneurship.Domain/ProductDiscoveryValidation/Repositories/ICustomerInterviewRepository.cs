using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories
{
    /// <summary>
    /// Repository interface for CustomerInterview aggregate
    /// </summary>
    public interface ICustomerInterviewRepository : IRepository<CustomerInterview, Guid>
    {
        /// <summary>
        /// Finds interviews matching the given specification
        /// </summary>
        Task<List<CustomerInterview>> GetListAsync(
            Expression<Func<CustomerInterview, bool>> predicate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds interviews by customer segment
        /// </summary>
        Task<List<CustomerInterview>> GetByCustomerSegmentAsync(
            Guid customerSegmentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds interviews by status
        /// </summary>
        Task<List<CustomerInterview>> GetByStatusAsync(
            InterviewStatus status,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds interviews scheduled within a date range
        /// </summary>
        Task<List<CustomerInterview>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds interviews with high-value insights
        /// </summary>
        Task<List<CustomerInterview>> GetWithHighValueInsightsAsync(
            int minimumInsightCount = 3,
            int minimumImportance = 4,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets interview statistics for dashboard
        /// </summary>
        Task<InterviewStatistics> GetStatisticsAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets interviews with their transcripts and insights
        /// </summary>
        Task<CustomerInterview> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches interviews by participant or content
        /// </summary>
        Task<List<CustomerInterview>> SearchAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets recently completed interviews needing analysis
        /// </summary>
        Task<List<CustomerInterview>> GetNeedingAnalysisAsync(
            int daysSinceCompletion = 7,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts interviews by specified criteria
        /// </summary>
        Task<int> CountAsync(
            Expression<Func<CustomerInterview, bool>> predicate,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Data transfer object for interview statistics
    /// </summary>
    public class InterviewStatistics
    {
        public int TotalInterviews { get; set; }
        public int CompletedInterviews { get; set; }
        public int ScheduledInterviews { get; set; }
        public int CancelledInterviews { get; set; }
        public int TotalInsights { get; set; }
        public decimal AverageInsightsPer Interview { get; set; }
        public Dictionary<string, int> InterviewsBySegment { get; set; }
        public Dictionary<InterviewType, int> InterviewsByType { get; set; }
        public Dictionary<string, int> TopInsightCategories { get; set; }
    }
}
