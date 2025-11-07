using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Infrastructure.SearchEngine.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.SearchEngine.Data
{
    public interface ISearchHistoryRepository : IRepository<SearchHistory, Guid>
    {
        Task<List<SearchHistory>> GetRecentByUserAsync(Guid userId, int maxResults = 50);
        Task<List<SearchHistory>> GetListAsync(ISpecification<SearchHistory> spec, int maxResultCount = 100);
        Task<List<SearchHistory>> GetZeroResultSearchesAsync(Guid userId);
        Task DeleteOldHistoryAsync(DateTime olderThan);
    }
}
