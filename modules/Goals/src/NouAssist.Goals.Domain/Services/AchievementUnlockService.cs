using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Core.Events;
using NouAssist.Goals.Aggregates;
using NouAssist.Goals.Data;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;

namespace NouAssist.Goals.Services
{
    public class AchievementUnlockService : DomainService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserAchievementRepository _userAchievementRepository;
        private readonly IDistributedEventBus _distributedEventBus;

        public AchievementUnlockService(
            IAchievementRepository achievementRepository,
            IUserAchievementRepository userAchievementRepository,
            IDistributedEventBus distributedEventBus)
        {
            _achievementRepository = achievementRepository;
            _userAchievementRepository = userAchievementRepository;
            _distributedEventBus = distributedEventBus;
        }

        public async Task<UserAchievement?> TryUnlockAchievementAsync(
            Guid userId, 
            Guid achievementId,
            Guid? tenantId = null)
        {
            // Check if already unlocked
            var existing = await _userAchievementRepository
                .FindByUserAndAchievementAsync(userId, achievementId);

            if (existing != null && existing.IsFullyUnlocked)
                return null;

            var achievement = await _achievementRepository.GetAsync(achievementId);
            
            if (!achievement.IsActive)
                return null;

            var userAchievement = existing ?? new UserAchievement(
                GuidGenerator.Create(),
                userId,
                achievementId,
                100.0
            );

            if (existing == null)
            {
                await _userAchievementRepository.InsertAsync(userAchievement);
            }
            else
            {
                userAchievement.UpdateProgress(100.0);
                await _userAchievementRepository.UpdateAsync(userAchievement);
            }

            await _distributedEventBus.PublishAsync(new AchievementUnlockedEto
            {
                UserAchievementId = userAchievement.Id,
                UserId = userId,
                AchievementId = achievementId,
                TenantId = tenantId,
                UnlockedAt = userAchievement.UnlockedAt,
                Points = achievement.Points
            });

            return userAchievement;
        }

        public async Task UpdateAchievementProgressAsync(
            Guid userId,
            Guid achievementId,
            double progressPercentage)
        {
            var userAchievement = await _userAchievementRepository
                .FindByUserAndAchievementAsync(userId, achievementId);

            if (userAchievement == null)
            {
                userAchievement = new UserAchievement(
                    GuidGenerator.Create(),
                    userId,
                    achievementId,
                    progressPercentage
                );
                await _userAchievementRepository.InsertAsync(userAchievement);
            }
            else
            {
                userAchievement.UpdateProgress(progressPercentage);
                await _userAchievementRepository.UpdateAsync(userAchievement);
            }

            if (userAchievement.IsFullyUnlocked && !userAchievement.IsNotified)
            {
                var achievement = await _achievementRepository.GetAsync(achievementId);
                
                await _distributedEventBus.PublishAsync(new AchievementUnlockedEto
                {
                    UserAchievementId = userAchievement.Id,
                    UserId = userId,
                    AchievementId = achievementId,
                    UnlockedAt = userAchievement.UnlockedAt,
                    Points = achievement.Points
                });
            }
        }
    }
}
