using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using NouAssist.Infrastructure.Search.Aggregates;

namespace NouAssist.Infrastructure.Search.Data
{
    public interface ISearchAnalyticsRepository : IRepository<SearchAnalytics, Guid>
    {
        Task<List<SearchAnalytics>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<List<SearchAnalytics>> GetByUserAsync(Guid userId, int maxResults = 100);
        Task<double> GetAverageQualityScoreAsync(Guid userId);
        Task<int> GetTotalSearchCountAsync(Guid userId);
    }
}