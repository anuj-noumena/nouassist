using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using NouAssist.Infrastructure.Search.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.Search.Data
{
    public interface IPopularSearchRepository : IRepository<PopularSearch, Guid>
    {
        // Add custom repository methods if needed
        Task<PopularSearch> FindByQueryAsync(string query, CancellationToken cancellationToken = default);
        Task<List<PopularSearch>> GetTopSearchesAsync(int maxResults = 10);
        Task<List<PopularSearch>> GetListAsync(ISpecification<PopularSearch> spec, int maxResultCount = 100);
    }
}