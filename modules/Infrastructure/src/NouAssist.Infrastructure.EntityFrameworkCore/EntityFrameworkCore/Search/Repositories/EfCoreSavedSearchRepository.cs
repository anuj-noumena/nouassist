using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using NouAssist.Infrastructure.Search.Aggregates;
using NouAssist.Infrastructure.Search.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NouAssist.Infrastructure.EntityFrameworkCore.Search.Repositories
{
    public class EfCoreSavedSearchRepository
        : EfCoreRepository<IInfrastructureDbContext, SavedSearch, Guid>, ISavedSearchRepository
    {
        public EfCoreSavedSearchRepository(
            IDbContextProvider<IInfrastructureDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SavedSearch>> GetByUserAsync(Guid userId)
        {
            return await (await GetQueryableAsync())
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreationTime)
                .ToListAsync();
        }

        public async Task<SavedSearch> FindByNameAsync(Guid userId, string name)
        {
            return await (await GetQueryableAsync())
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Name == name);
        }
    }
}
