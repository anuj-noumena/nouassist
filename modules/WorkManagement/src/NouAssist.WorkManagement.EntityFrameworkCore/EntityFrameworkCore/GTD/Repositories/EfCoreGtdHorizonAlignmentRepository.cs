using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using NouAssist.WorkManagement.GTD.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.GTD.Repositories
{
    public class EfCoreGtdHorizonAlignmentRepository 
        : EfCoreRepository<IWorkManagementDbContext, GtdHorizonAlignment, Guid>, IGtdHorizonAlignmentRepository
    {
        public EfCoreGtdHorizonAlignmentRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<GtdHorizonAlignment?> FindByHorizonAndEntityAsync(
            Guid higherHorizonId,
            string lowerEntityType,
            Guid lowerEntityId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(a => a.HigherHorizonId == higherHorizonId &&
                                         a.LowerEntityType == lowerEntityType &&
                                         a.LowerEntityId == lowerEntityId);
        }

        public async Task<List<GtdHorizonAlignment>> GetByUserAndLevelAsync(
            Guid userId,
            GtdHorizonLevel level)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(a => a.OwnerId == userId && a.HigherLevel == level)
                .ToListAsync();
        }

        public async Task<List<GtdHorizonAlignment>> GetWeakAlignmentsAsync(
            Guid userId,
            int weakThreshold = 2)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(a => a.OwnerId == userId && 
                           a.AlignmentStrength.HasValue && 
                           a.AlignmentStrength.Value <= weakThreshold)
                .ToListAsync();
        }

        public async Task<List<GtdHorizonAlignment>> GetByHorizonAsync(Guid horizonId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(a => a.HigherHorizonId == horizonId)
                .ToListAsync();
        }

        public async Task<List<GtdHorizonAlignment>> GetByEntityAsync(
            string entityType,
            Guid entityId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(a => a.LowerEntityType == entityType && a.LowerEntityId == entityId)
                .ToListAsync();
        }
    }
}
