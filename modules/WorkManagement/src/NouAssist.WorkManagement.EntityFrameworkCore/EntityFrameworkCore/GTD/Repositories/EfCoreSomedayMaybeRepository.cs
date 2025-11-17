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
    public class EfCoreSomedayMaybeRepository 
        : EfCoreRepository<IWorkManagementDbContext, SomedayMaybeItem, Guid>, ISomedayMaybeRepository
    {
        public EfCoreSomedayMaybeRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SomedayMaybeItem>> GetActiveByUserAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(s => s.OwnerId == userId && s.Status == SomedayMaybeStatus.Active)
                .OrderByDescending(s => s.CreationTime)
                .ToListAsync();
        }

        public async Task<List<SomedayMaybeItem>> GetNeedingReviewAsync(Guid userId)
        {
            var reviewThreshold = DateTime.UtcNow.AddDays(-90);
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(s => s.OwnerId == userId && 
                           s.Status == SomedayMaybeStatus.Active &&
                           (!s.LastReviewedAt.HasValue || s.LastReviewedAt.Value < reviewThreshold))
                .OrderBy(s => s.LastReviewedAt)
                .ToListAsync();
        }
    }
}
