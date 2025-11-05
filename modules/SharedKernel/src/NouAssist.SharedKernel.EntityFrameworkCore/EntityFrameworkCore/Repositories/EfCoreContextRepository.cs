using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Core.Aggregates;
using NouAssist.Core.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Core.EntityFrameworkCore.Repositories
{
    public class EfCoreContextRepository 
        : EfCoreRepository<ICoreDbContext, Context, Guid>, IContextRepository
    {
        public EfCoreContextRepository(
            IDbContextProvider<ICoreDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Context>> GetActiveByUserAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(c => c.OwnerId == userId && c.IsActive && !c.IsDeleted)
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public async Task<List<Context>> GetByLocationAsync(Guid userId, string location)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(c => c.OwnerId == userId && c.Location == location && c.IsActive)
                .ToListAsync();
        }
    }
}
