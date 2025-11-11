using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Infrastructure.Search.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Infrastructure.Search.Data
{
    public interface ISearchAnalyticsRepository : IRepository<SearchAnalytics, Guid>
    {
        Task<List<SearchAnalytics>> GetByUserAsync(Guid userId, int maxResults = 100);
        Task<List<SearchAnalytics>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<double> GetAverageQualityScoreAsync(Guid userId);
        Task<int> GetTotalSearchCountAsync(Guid userId);
    }
}
