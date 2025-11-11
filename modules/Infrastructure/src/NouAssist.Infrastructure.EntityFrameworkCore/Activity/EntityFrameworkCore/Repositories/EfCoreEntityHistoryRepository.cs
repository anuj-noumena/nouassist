using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Infrastructure.Activity.Aggregates;
using NouAssist.Infrastructure.Activity.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Infrastructure.Activity.EntityFrameworkCore.Repositories
{
    public class EfCoreEntityHistoryRepository 
        : EfCoreRepository<IActivityDbContext, EntityHistory, Guid>, IEntityHistoryRepository
    {
        public EfCoreEntityHistoryRepository(
            IDbContextProvider<IActivityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<EntityHistory>> GetEntityHistoryAsync(
            string entityType, 
            Guid entityId, 
            int maxResultCount = 50)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(h => h.EntityType == entityType && h.EntityId == entityId)
                .OrderByDescending(h => h.Version)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<EntityHistory> GetLatestVersionAsync(string entityType, Guid entityId)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(h => h.EntityType == entityType && h.EntityId == entityId)
                .OrderByDescending(h => h.Version)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetNextVersionNumberAsync(string entityType, Guid entityId)
        {
            var latestVersion = await GetLatestVersionAsync(entityType, entityId);
            return latestVersion != null ? latestVersion.Version + 1 : 1;
        }
    }
}
