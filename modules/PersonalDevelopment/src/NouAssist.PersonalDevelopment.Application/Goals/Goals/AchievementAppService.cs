using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.PersonalDevelopment.Goals.Aggregates;
using NouAssist.PersonalDevelopment.Goals.Data;
using NouAssist.PersonalDevelopment.Goals.Dtos;
using NouAssist.PersonalDevelopment.Goals.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.PersonalDevelopment.Goals.Goals
{
    [Authorize(GoalsPermissions.Achievements.Default)]
    public class AchievementAppService : ApplicationService, IAchievementAppService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserAchievementRepository _userAchievementRepository;

        public AchievementAppService(
            IAchievementRepository achievementRepository,
            IUserAchievementRepository userAchievementRepository)
        {
            _achievementRepository = achievementRepository;
            _userAchievementRepository = userAchievementRepository;
        }

        [Authorize(GoalsPermissions.Achievements.View)]
        public async Task<ListResultDto<AchievementDto>> GetActiveAchievementsAsync()
        {
            var achievements = await _achievementRepository.GetActiveAchievementsAsync();
            var dtos = ObjectMapper.Map<List<Achievement>, List<AchievementDto>>(achievements);
            return new ListResultDto<AchievementDto>(dtos);
        }

        public async Task<ListResultDto<UserAchievementDto>> GetMyAchievementsAsync()
        {
            var userId = CurrentUser.GetId();
            var userAchievements = await _userAchievementRepository.GetByUserAsync(userId);
            
            // Load achievement details
            var achievementIds = userAchievements.Select(ua => ua.AchievementId).ToList();
            var achievements = await _achievementRepository.GetListAsync(achievementIds);
            
            var dtos = userAchievements.Select(ua =>
            {
                var dto = ObjectMapper.Map<UserAchievement, UserAchievementDto>(ua);
                dto.Achievement = ObjectMapper.Map<Achievement, AchievementDto>(
                    achievements.FirstOrDefault(a => a.Id == ua.AchievementId));
                return dto;
            }).ToList();

            return new ListResultDto<UserAchievementDto>(dtos);
        }

        public async Task<int> GetMyTotalPointsAsync()
        {
            var userId = CurrentUser.GetId();
            return await _userAchievementRepository.GetTotalPointsByUserAsync(userId);
        }
    }
}
