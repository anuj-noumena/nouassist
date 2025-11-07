using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Core.Enums;
using NouAssist.CoreProductivity.GoalTracking.Aggregates;
using NouAssist.CoreProductivity.GoalTracking.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.GoalTracking.EntityFrameworkCore.Repositories
{
    public class EfCoreAchievementRepository 
        : EfCoreRepository<IGoalsDbContext, Achievement, Guid>, IAchievementRepository
    {
        public EfCoreAchievementRepository(
            IDbContextProvider<IGoalsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Achievement>> GetActiveAchievementsAsync()
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(a => a.IsActive)
                .OrderBy(a => a.Category)
                .ThenByDescending(a => a.Points)
                .ToListAsync();
        }

        public async Task<List<Achievement>> GetByCategoryAsync(AchievementCategory category)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(a => a.Category == category && a.IsActive)
                .OrderByDescending(a => a.Points)
                .ToListAsync();
        }
    }
}
