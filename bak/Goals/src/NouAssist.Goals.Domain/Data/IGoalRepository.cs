using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Core.Enums;
using NouAssist.Goals.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Goals.Data
{
    public interface IGoalRepository : IRepository<Goal, Guid>
    {
        Task<Goal> GetWithMilestonesAsync(Guid id);
        Task<List<Goal>> GetListAsync(ISpecification<Goal> spec);
        Task<List<Goal>> GetByUserAsync(Guid userId);
        Task<List<Goal>> GetByAreaAsync(Guid areaId);
        Task<int> CountActiveByUserAsync(Guid userId);
    }

    public interface IAchievementRepository : IRepository<Achievement, Guid>
    {
        Task<List<Achievement>> GetActiveAchievementsAsync();
        Task<List<Achievement>> GetByCategoryAsync(AchievementCategory category);
    }

    public interface IUserAchievementRepository : IRepository<UserAchievement, Guid>
    {
        Task<UserAchievement?> FindByUserAndAchievementAsync(Guid userId, Guid achievementId);
        Task<List<UserAchievement>> GetByUserAsync(Guid userId);
        Task<int> GetTotalPointsByUserAsync(Guid userId);
    }
}
