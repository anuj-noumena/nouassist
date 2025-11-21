using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of CustomerSegment repository
    /// </summary>
    public class EfCoreCustomerSegmentRepository 
        : EfCoreRepository<IProductDiscoveryDbContext, CustomerSegment, Guid>, ICustomerSegmentRepository
    {
        public EfCoreCustomerSegmentRepository(
            IDbContextProvider<IProductDiscoveryDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<CustomerSegment>> GetListAsync(
            Expression<Func<CustomerSegment, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(predicate)
                .OrderByDescending(s => s.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CustomerSegment>> GetCompletedSegmentsAsync(
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                s => s.ActualSampleSize >= s.TargetSampleSize,
                cancellationToken);
        }

        public async Task<List<CustomerSegment>> GetLowCompletionSegmentsAsync(
            decimal maximumCompletionPercentage = 50m,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(s => s.TargetSampleSize > 0 &&
                           (decimal)s.ActualSampleSize / s.TargetSampleSize * 100 <= maximumCompletionPercentage)
                .OrderBy(s => (decimal)s.ActualSampleSize / s.TargetSampleSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CustomerSegment>> GetRecentSegmentsAsync(
            int daysAgo = 30,
            CancellationToken cancellationToken = default)
        {
            var startDate = DateTime.UtcNow.AddDays(-daysAgo);

            return await GetListAsync(
                s => s.CreationTime >= startDate,
                cancellationToken);
        }

        public async Task<SegmentCompletionSummary> GetCompletionSummaryAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            var segments = await query.ToListAsync(cancellationToken);

            var totalSegments = segments.Count;
            var completedSegments = segments.Count(s => s.ActualSampleSize >= s.TargetSampleSize);
            var inProgressSegments = segments.Count(s => 
                s.ActualSampleSize > 0 && s.ActualSampleSize < s.TargetSampleSize);
            var notStartedSegments = segments.Count(s => s.ActualSampleSize == 0);

            var totalTargetSampleSize = segments.Sum(s => s.TargetSampleSize);
            var totalActualSampleSize = segments.Sum(s => s.ActualSampleSize);
            var overallCompletionRate = totalTargetSampleSize > 0 
                ? (decimal)totalActualSampleSize / totalTargetSampleSize * 100 
                : 0;

            var segmentsNeedingAttention = GetSegmentsNeedingAttention(segments)
                .Select(s => s.Name)
                .ToList();

            return new SegmentCompletionSummary
            {
                TotalSegments = totalSegments,
                CompletedSegments = completedSegments,
                InProgressSegments = inProgressSegments,
                NotStartedSegments = notStartedSegments,
                OverallCompletionRate = overallCompletionRate,
                TotalTargetSampleSize = totalTargetSampleSize,
                TotalActualSampleSize = totalActualSampleSize,
                SegmentsNeedingAttention = segmentsNeedingAttention
            };
        }

        public async Task<List<CustomerSegment>> SearchAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<CustomerSegment>();

            var query = await GetQueryableAsync();
            searchTerm = searchTerm.ToLower();

            return await query
                .Where(s => s.Name.ToLower().Contains(searchTerm) ||
                           s.Description.ToLower().Contains(searchTerm))
                .OrderByDescending(s => s.CreationTime)
                .Take(maxResults)
                .ToListAsync(cancellationToken);
        }

        public async Task<SegmentStatistics> GetStatisticsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            var segments = await query.ToListAsync(cancellationToken);

            var totalSegments = segments.Count;
            var avgTargetSampleSize = segments.Any() ? segments.Average(s => s.TargetSampleSize) : 0;
            var avgActualSampleSize = segments.Any() ? segments.Average(s => s.ActualSampleSize) : 0;
            var avgCompletionPercentage = segments.Any() 
                ? segments.Average(s => s.CompletionPercentage()) 
                : 0;

            var segmentsByCompletion = segments
                .GroupBy(s => s.CompletionPercentage() switch
                {
                    >= 100 => "Completed",
                    >= 50 => "In Progress",
                    > 0 => "Started",
                    _ => "Not Started"
                })
                .ToDictionary(g => g.Key, g => g.Count());

            var topPerformingSegments = segments
                .Where(s => s.CompletionPercentage() >= 80)
                .OrderByDescending(s => s.CompletionPercentage())
                .Take(5)
                .Select(s => s.Name)
                .ToList();

            var underperformingSegments = segments
                .Where(s => s.CompletionPercentage() < 50 && s.CreationTime <= DateTime.UtcNow.AddDays(-30))
                .OrderBy(s => s.CompletionPercentage())
                .Take(5)
                .Select(s => s.Name)
                .ToList();

            var segmentsNeedingAttention = GetSegmentsNeedingAttention(segments).Count;

            return new SegmentStatistics
            {
                TotalSegments = totalSegments,
                AverageTargetSampleSize = avgTargetSampleSize,
                AverageActualSampleSize = avgActualSampleSize,
                AverageCompletionPercentage = avgCompletionPercentage,
                SegmentsByCompletion = segmentsByCompletion,
                TopPerformingSegments = topPerformingSegments,
                UnderperformingSegments = underperformingSegments,
                SegmentsNeedingAttention = segmentsNeedingAttention
            };
        }

        public async Task<List<CustomerSegment>> GetNeedingAttentionAsync(
            CancellationToken cancellationToken = default)
        {
            var segments = await GetListAsync(s => true, cancellationToken);
            return GetSegmentsNeedingAttention(segments);
        }

        public async Task<List<CustomerSegment>> FindByAttributesAsync(
            Dictionary<string, object> attributes,
            CancellationToken cancellationToken = default)
        {
            var segments = await GetListAsync(s => true, cancellationToken);
            
            return segments
                .Where(s => s.Criteria.Matches(attributes))
                .ToList();
        }

        private List<CustomerSegment> GetSegmentsNeedingAttention(List<CustomerSegment> segments)
        {
            var needingAttention = new List<CustomerSegment>();

            foreach (var segment in segments)
            {
                // Low completion rate after reasonable time
                if (segment.CreationTime <= DateTime.UtcNow.AddDays(-30) && 
                    segment.CompletionPercentage() < 30)
                {
                    needingAttention.Add(segment);
                    continue;
                }

                // No progress for extended period
                if (segment.CreationTime <= DateTime.UtcNow.AddDays(-60) && 
                    segment.ActualSampleSize == 0)
                {
                    needingAttention.Add(segment);
                    continue;
                }

                // Stalled progress
                if (segment.ActualSampleSize > 0 && 
                    segment.CompletionPercentage() < 100 &&
                    segment.LastModificationTime.HasValue &&
                    segment.LastModificationTime.Value <= DateTime.UtcNow.AddDays(-14))
                {
                    needingAttention.Add(segment);
                }
            }

            return needingAttention;
        }
    }
}
