using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.GTD.Aggregates;
using NouAssist.GTD.Data;
using NouAssist.GTD.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.GTD.EntityFrameworkCore.Repositories
{
    public class EfCoreSomedayMaybeRepository 
        : EfCoreRepository<IGtdDbContext, SomedayMaybeItem, Guid>, ISomedayMaybeRepository
    {
        public EfCoreSomedayMaybeRepository(
            IDbContextProvider<IGtdDbContext> dbContextProvider)
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
