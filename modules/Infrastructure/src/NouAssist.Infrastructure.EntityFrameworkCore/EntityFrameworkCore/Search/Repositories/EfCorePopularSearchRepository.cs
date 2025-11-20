using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using NouAssist.Infrastructure.Search.Aggregates;
using NouAssist.Infrastructure.Search.Data;
using Volo.Abp.Specifications;
using System.Linq;

namespace NouAssist.Infrastructure.EntityFrameworkCore.Search.Repositories
{
    public class EfCorePopularSearchRepository
        : EfCoreRepository<IInfrastructureDbContext, PopularSearch, Guid>, IPopularSearchRepository
    {
        public EfCorePopularSearchRepository(
            IDbContextProvider<IInfrastructureDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<PopularSearch> FindByQueryAsync(string query, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                .FirstOrDefaultAsync(p => p.Query == query, GetCancellationToken(cancellationToken));
        }

        public async Task<List<PopularSearch>> GetTopSearchesAsync(int maxResults = 10)
        {
            return await (await GetQueryableAsync())
                .OrderByDescending(p => p.SearchCount)
                .Take(maxResults)
                .ToListAsync();
        }

        public async Task<List<PopularSearch>> GetListAsync(ISpecification<PopularSearch> spec, int maxResultCount = 100)
        {
            return await (await GetQueryableAsync())
                .Where(spec.ToExpression())
                .OrderByDescending(p => p.SearchCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
