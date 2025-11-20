using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using NouAssist.Infrastructure.Search.Aggregates;
using NouAssist.Infrastructure.Search.Data;
using Volo.Abp.Specifications;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace NouAssist.Infrastructure.EntityFrameworkCore.Search.Repositories
{
    public class EfCoreSearchHistoryRepository
        : EfCoreRepository<IInfrastructureDbContext, SearchHistory, Guid>, ISearchHistoryRepository
    {
        public EfCoreSearchHistoryRepository(
            IDbContextProvider<IInfrastructureDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SearchHistory>> GetRecentByUserAsync(Guid userId, int maxResults = 50)
        {
            return await (await GetQueryableAsync())
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.CreationTime)
                .Take(maxResults)
                .ToListAsync();
        }

        public async Task<List<SearchHistory>> GetListAsync(ISpecification<SearchHistory> spec, int maxResultCount = 100)
        {
            return await (await GetQueryableAsync())
                .Where(spec.ToExpression())
                .OrderByDescending(h => h.CreationTime)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<List<SearchHistory>> GetZeroResultSearchesAsync(Guid userId)
        {
            return await (await GetQueryableAsync())
                .Where(h => h.UserId == userId && h.ResultCount == 0)
                .ToListAsync();
        }

        public async Task DeleteOldHistoryAsync(DateTime olderThan)
        {
            var oldHistory = await (await GetQueryableAsync())
                .Where(h => h.CreationTime < olderThan)
                .ToListAsync();
            
            await DeleteManyAsync(oldHistory);
        }

        public async Task<List<SearchHistory>> GetByUserAsync(Guid userId)
        {
            return await (await GetQueryableAsync())
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.CreationTime)
                .ToListAsync();
        }
    }
}
