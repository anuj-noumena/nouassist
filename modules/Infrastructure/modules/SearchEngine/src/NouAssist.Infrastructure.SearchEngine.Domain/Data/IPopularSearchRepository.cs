using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Infrastructure.SearchEngine.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.SearchEngine.Data
{
    public interface IPopularSearchRepository : IRepository<PopularSearch, Guid>
    {
        Task<List<PopularSearch>> GetTopSearchesAsync(int maxResults = 10);
        Task<PopularSearch> FindByQueryAsync(string query);
        Task<List<PopularSearch>> GetListAsync(ISpecification<PopularSearch> spec, int maxResultCount = 100);
    }
}
