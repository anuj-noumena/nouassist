using System;
using NouAssist.SharedKernel.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.PersonalDevelopment.Goals.Aggregates
{
    public class Achievement : FullAuditedAggregateRoot<Guid>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string? Icon { get; set; }
        public string? BadgeImageUrl { get; set; }
        public AchievementCategory Category { get; private set; }
        public AchievementRarity Rarity { get; private set; }
        public int Points { get; private set; }
        public string? CriteriaJson { get; set; }
        public bool IsSecret { get; set; }
        public bool IsActive { get; set; }

        protected Achievement() { }

        public Achievement(
            Guid id, 
            string title, 
            string description,
            AchievementCategory category,
            int points = 10,
            AchievementRarity rarity = AchievementRarity.Common) : base(id)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 255);
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 5000);
            Category = category;
            Rarity = rarity;
            
            if (points < 1 || points > 1000)
            {
                throw new BusinessException("Goals:InvalidAchievementPoints", 
                    "Achievement points must be between 1 and 1000.");
            }
            Points = points;
            IsActive = true;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void UpdatePoints(int points)
        {
            if (points < 1 || points > 1000)
            {
                throw new BusinessException("Goals:InvalidAchievementPoints", 
                    "Achievement points must be between 1 and 1000.");
            }
            Points = points;
        }

        public string GetRarityDescription()
        {
            return Rarity switch
            {
                AchievementRarity.Common => "Common",
                AchievementRarity.Uncommon => "Uncommon",
                AchievementRarity.Rare => "Rare",
                AchievementRarity.Epic => "Epic",
                AchievementRarity.Legendary => "Legendary",
                _ => "Unknown"
            };
        }
    }

    public class UserAchievement : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public Guid AchievementId { get; private set; }
        public DateTime UnlockedAt { get; private set; }
        public double ProgressPercentage { get; private set; }
        public string? ProgressDataJson { get; set; }
        public bool IsNotified { get; private set; }
        public DateTime? NotifiedAt { get; private set; }

        protected UserAchievement() { }

        public UserAchievement(
            Guid id, 
            Guid userId, 
            Guid achievementId, 
            double progressPercentage = 100.0) : base(id)
        {
            UserId = userId;
            AchievementId = achievementId;
            UnlockedAt = DateTime.UtcNow;
            
            if (progressPercentage < 0 || progressPercentage > 100)
            {
                throw new BusinessException("Goals:InvalidProgress", 
                    "Progress percentage must be between 0 and 100.");
            }
            ProgressPercentage = progressPercentage;
            IsNotified = false;
        }

        public void UpdateProgress(double percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new BusinessException("Goals:InvalidProgress", 
                    "Progress percentage must be between 0 and 100.");
            }
            ProgressPercentage = Math.Clamp(percentage, 0, 100);
        }

        public void MarkAsNotified()
        {
            if (IsNotified)
            {
                return;
            }

            IsNotified = true;
            NotifiedAt = DateTime.UtcNow;
        }

        public bool IsFullyUnlocked => ProgressPercentage >= 100.0;
        public bool IsPartiallyUnlocked => ProgressPercentage > 0 && ProgressPercentage < 100;
        public TimeSpan TimeSinceUnlock => DateTime.UtcNow - UnlockedAt;
        public int DaysSinceUnlock => TimeSinceUnlock.Days;
    }
}
