using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using NouAssist.Infrastructure.Search.Aggregates;
using NouAssist.Infrastructure.Search.Data;

namespace NouAssist.Infrastructure.EntityFrameworkCore.Search.Repositories
{
    public class EfCoreSearchAnalyticsRepository
        : EfCoreRepository<IInfrastructureDbContext, SearchAnalytics, Guid>, ISearchAnalyticsRepository
    {
        public EfCoreSearchAnalyticsRepository(
            IDbContextProvider<IInfrastructureDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SearchAnalytics>> GetByDateRangeAsync(
            Guid userId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                .Where(sa => sa.UserId == userId && sa.Timestamp >= startDate && sa.Timestamp <= endDate)
                .OrderBy(sa => sa.Timestamp)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<SearchAnalytics>> GetByUserAsync(Guid userId, int maxResults = 100)
        {
            return await (await GetQueryableAsync())
                .Where(sa => sa.UserId == userId)
                .OrderByDescending(sa => sa.Timestamp)
                .Take(maxResults)
                .ToListAsync();
        }

        public async Task<double> GetAverageQualityScoreAsync(Guid userId)
        {
            return await (await GetQueryableAsync())
                .Where(sa => sa.UserId == userId)
                .AverageAsync(sa => sa.QualityScore);
        }

        public async Task<int> GetTotalSearchCountAsync(Guid userId)
        {
            return await (await GetQueryableAsync())
                .CountAsync(sa => sa.UserId == userId);
        }
    }
}
