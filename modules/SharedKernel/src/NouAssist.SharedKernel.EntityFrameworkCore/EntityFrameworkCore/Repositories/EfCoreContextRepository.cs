using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.SharedKernel.Aggregates;
using NouAssist.SharedKernel.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.SharedKernel.EntityFrameworkCore.Repositories
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
