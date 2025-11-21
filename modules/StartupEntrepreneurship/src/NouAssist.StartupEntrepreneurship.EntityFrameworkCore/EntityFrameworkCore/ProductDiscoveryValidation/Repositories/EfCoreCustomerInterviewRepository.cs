using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Exceptions;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of CustomerInterview repository
    /// </summary>
    public class EfCoreCustomerInterviewRepository 
        : EfCoreRepository<IProductDiscoveryDbContext, CustomerInterview, Guid>, ICustomerInterviewRepository
    {
        public EfCoreCustomerInterviewRepository(
            IDbContextProvider<IProductDiscoveryDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<CustomerInterview>> GetListAsync(
            Expression<Func<CustomerInterview, bool>> predicate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query
                    .Include(i => i.Transcripts)
                    .Include(i => i.Insights);
            }

            return await query
                .Where(predicate)
                .OrderByDescending(i => i.ScheduledDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CustomerInterview>> GetByCustomerSegmentAsync(
            Guid customerSegmentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                i => i.CustomerSegmentId == customerSegmentId,
                includeDetails,
                cancellationToken);
        }

        public async Task<List<CustomerInterview>> GetByStatusAsync(
            InterviewStatus status,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                i => i.Status == status,
                includeDetails,
                cancellationToken);
        }

        public async Task<List<CustomerInterview>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                i => i.ScheduledDate >= startDate && i.ScheduledDate <= endDate,
                includeDetails,
                cancellationToken);
        }

        public async Task<List<CustomerInterview>> GetWithHighValueInsightsAsync(
            int minimumInsightCount = 3,
            int minimumImportance = 4,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Include(i => i.Insights)
                .Where(i => i.Status == InterviewStatus.Completed &&
                           i.Insights.Count >= minimumInsightCount &&
                           i.Insights.Any(insight => insight.Importance >= minimumImportance))
                .OrderByDescending(i => i.Insights.Average(insight => insight.Importance))
                .ToListAsync(cancellationToken);
        }

        public async Task<InterviewStatistics> GetStatisticsAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (fromDate.HasValue)
                query = query.Where(i => i.ScheduledDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(i => i.ScheduledDate <= toDate.Value);

            var interviews = await query
                .Include(i => i.Insights)
                .ToListAsync(cancellationToken);

            var totalInterviews = interviews.Count;
            var completedInterviews = interviews.Count(i => i.Status == InterviewStatus.Completed);
            var scheduledInterviews = interviews.Count(i => i.Status == InterviewStatus.Scheduled);
            var cancelledInterviews = interviews.Count(i => i.Status == InterviewStatus.Cancelled);

            var allInsights = interviews.SelectMany(i => i.Insights).ToList();
            var totalInsights = allInsights.Count;
            var avgInsightsPerInterview = completedInterviews > 0 ? (decimal)totalInsights / completedInterviews : 0;

            var interviewsBySegment = interviews
                .Where(i => i.CustomerSegmentId.HasValue)
                .GroupBy(i => i.CustomerSegmentId.Value)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            var interviewsByType = interviews
                .GroupBy(i => i.Type)
                .ToDictionary(g => g.Key, g => g.Count());

            var topInsightCategories = allInsights
                .GroupBy(i => i.Category)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .ToDictionary(g => g.Key, g => g.Count());

            return new InterviewStatistics
            {
                TotalInterviews = totalInterviews,
                CompletedInterviews = completedInterviews,
                ScheduledInterviews = scheduledInterviews,
                CancelledInterviews = cancelledInterviews,
                TotalInsights = totalInsights,
                AverageInsightsPerInterview = avgInsightsPerInterview,
                InterviewsBySegment = interviewsBySegment,
                InterviewsByType = interviewsByType,
                TopInsightCategories = topInsightCategories
            };
        }

        public async Task<CustomerInterview> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            var interview = await query
                .Include(i => i.Transcripts)
                .Include(i => i.Insights)
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

            if (interview == null)
                throw new CustomerInterviewNotFoundException(id);

            return interview;
        }

        public async Task<List<CustomerInterview>> SearchAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<CustomerInterview>();

            var query = await GetQueryableAsync();
            searchTerm = searchTerm.ToLower();

            return await query
                .Where(i => i.Title.ToLower().Contains(searchTerm) ||
                           i.Description.ToLower().Contains(searchTerm) ||
                           i.CustomerName.ToLower().Contains(searchTerm) ||
                           i.CustomerEmail.ToLower().Contains(searchTerm))
                .OrderByDescending(i => i.ScheduledDate)
                .Take(maxResults)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CustomerInterview>> GetNeedingAnalysisAsync(
            int daysSinceCompletion = 7,
            CancellationToken cancellationToken = default)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysSinceCompletion);

            return await GetListAsync(
                i => i.Status == InterviewStatus.Completed &&
                     i.CompletedDate.HasValue &&
                     i.CompletedDate.Value <= cutoffDate,
                true,
                cancellationToken);
        }

        public async Task<int> CountAsync(
            Expression<Func<CustomerInterview, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query.Where(predicate).CountAsync(cancellationToken);
        }

        public override async Task<IQueryable<CustomerInterview>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(i => i.Transcripts)
                .Include(i => i.Insights);
        }
    }
}
