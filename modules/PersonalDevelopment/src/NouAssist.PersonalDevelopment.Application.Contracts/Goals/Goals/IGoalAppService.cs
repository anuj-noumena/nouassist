using System;
using System.Threading.Tasks;
using NouAssist.Goals.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Goals.Goals
{
    public interface IGoalAppService : IApplicationService
    {
        Task<GoalDto> GetAsync(Guid id);
        Task<PagedResultDto<GoalDto>> GetListAsync(GetGoalListDto input);
        Task<GoalDto> CreateAsync(CreateGoalDto input);
        Task<GoalDto> UpdateAsync(Guid id, UpdateGoalDto input);
        Task DeleteAsync(Guid id);
        Task<GoalDto> UpdateProgressAsync(Guid id, UpdateGoalProgressDto input);
        Task<GoalDto> AchieveAsync(Guid id, AchieveGoalDto input);
        Task<GoalDto> PauseAsync(Guid id);
        Task<GoalDto> ResumeAsync(Guid id);
        Task<GoalDto> ArchiveAsync(Guid id, ArchiveGoalDto input);
        Task<GoalDto> AddMilestoneAsync(Guid goalId, AddGoalMilestoneDto input);
        Task RemoveMilestoneAsync(Guid goalId, Guid milestoneId);
        Task<GoalDto> CompleteMilestoneAsync(Guid goalId, Guid milestoneId);
        Task<GoalStatisticsDto> GetMyStatisticsAsync();
    }

    public interface IAchievementAppService : IApplicationService
    {
        Task<ListResultDto<AchievementDto>> GetActiveAchievementsAsync();
        Task<ListResultDto<UserAchievementDto>> GetMyAchievementsAsync();
        Task<int> GetMyTotalPointsAsync();
    }
}
