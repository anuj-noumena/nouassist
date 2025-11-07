using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Goals.Aggregates;
using NouAssist.Goals.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Goals.EntityFrameworkCore.Repositories
{
    public class EfCoreUserAchievementRepository 
        : EfCoreRepository<IGoalsDbContext, UserAchievement, Guid>, IUserAchievementRepository
    {
        public EfCoreUserAchievementRepository(
            IDbContextProvider<IGoalsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<UserAchievement?> FindByUserAndAchievementAsync(Guid userId, Guid achievementId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId);
        }

        public async Task<List<UserAchievement>> GetByUserAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(ua => ua.UserId == userId)
                .OrderByDescending(ua => ua.UnlockedAt)
                .ToListAsync();
        }

        public async Task<int> GetTotalPointsByUserAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await (from ua in dbContext.UserAchievements
                         join a in dbContext.Achievements on ua.AchievementId equals a.Id
                         where ua.UserId == userId && ua.IsFullyUnlocked
                         select a.Points).SumAsync();
        }
    }
}
