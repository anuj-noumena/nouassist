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
    public class EfCoreAreaRepository 
        : EfCoreRepository<ICoreDbContext, Area, Guid>, IAreaRepository
    {
        public EfCoreAreaRepository(
            IDbContextProvider<ICoreDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Area>> GetByUserAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(a => a.OwnerId == userId && !a.IsDeleted)
                .OrderBy(a => a.Title)
                .ToListAsync();
        }

        public async Task<List<Area>> GetRootAreasAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(a => a.OwnerId == userId && !a.ParentId.HasValue && !a.IsDeleted)
                .OrderBy(a => a.Title)
                .ToListAsync();
        }

        public async Task<List<Area>> GetChildAreasAsync(Guid parentId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(a => a.ParentId == parentId && !a.IsDeleted)
                .OrderBy(a => a.Title)
                .ToListAsync();
        }
    }
}
