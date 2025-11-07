using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkflowOrchestration.GTDSystem.Aggregates;
using NouAssist.WorkflowOrchestration.GTDSystem.Data;
using NouAssist.WorkflowOrchestration.GTDSystem.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.GTDSystem.EntityFrameworkCore.Repositories
{
    public class EfCoreGtdHorizonRepository 
        : EfCoreRepository<IGtdDbContext, GtdHorizon, Guid>, IGtdHorizonRepository
    {
        public EfCoreGtdHorizonRepository(
            IDbContextProvider<IGtdDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<GtdHorizon?> FindByUserAndLevelAsync(Guid userId, GtdHorizonLevel level)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(h => h.OwnerId == userId && h.HorizonLevel == level);
        }

        public async Task<List<GtdHorizon>> GetByUserAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(h => h.OwnerId == userId)
                .OrderBy(h => h.HorizonLevel)
                .ToListAsync();
        }

        public async Task<List<GtdHorizon>> GetNeedingReviewAsync(Guid userId)
        {
            var now = DateTime.UtcNow;
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(h => h.OwnerId == userId && 
                           h.NextReviewAt.HasValue && 
                           h.NextReviewAt.Value <= now)
                .OrderBy(h => h.NextReviewAt)
                .ToListAsync();
        }
    }
}
