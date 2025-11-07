using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Infrastructure.SearchEngine.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Infrastructure.SearchEngine.Data
{
    public interface ISearchAnalyticsRepository : IRepository<SearchAnalytics, Guid>
    {
        Task<List<SearchAnalytics>> GetByUserAsync(Guid userId, int maxResults = 100);
        Task<List<SearchAnalytics>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<double> GetAverageQualityScoreAsync(Guid userId);
        Task<int> GetTotalSearchCountAsync(Guid userId);
    }
}
