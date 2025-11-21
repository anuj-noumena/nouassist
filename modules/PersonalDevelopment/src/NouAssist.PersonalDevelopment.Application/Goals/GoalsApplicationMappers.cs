using NouAssist.PersonalDevelopment.Goals.Aggregates;
using NouAssist.PersonalDevelopment.Goals.Dtos;
using NouAssist.PersonalDevelopment.Goals.Entities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.PersonalDevelopment.Goals
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class GoalToGoalDtoMapper : MapperBase<Goal, GoalDto>
    {
        [MapperIgnoreTarget(nameof(GoalDto.ImportanceScore))]
        [MapperIgnoreTarget(nameof(GoalDto.UrgencyScore))]
        [MapperIgnoreTarget(nameof(GoalDto.EisenhowerQuadrant))]
        [MapperIgnoreTarget(nameof(GoalDto.IsOverdue))]
        [MapperIgnoreTarget(nameof(GoalDto.DaysRemaining))]
        [MapperIgnoreTarget(nameof(GoalDto.ProgressLevel))]
        [MapperIgnoreTarget(nameof(GoalDto.CompletedMilestonesCount))]
        [MapperIgnoreTarget(nameof(GoalDto.MilestoneCompletionRate))]
        public override partial GoalDto Map(Goal source);

        [MapperIgnoreTarget(nameof(GoalDto.ImportanceScore))]
        [MapperIgnoreTarget(nameof(GoalDto.UrgencyScore))]
        [MapperIgnoreTarget(nameof(GoalDto.EisenhowerQuadrant))]
        [MapperIgnoreTarget(nameof(GoalDto.IsOverdue))]
        [MapperIgnoreTarget(nameof(GoalDto.DaysRemaining))]
        [MapperIgnoreTarget(nameof(GoalDto.ProgressLevel))]
        [MapperIgnoreTarget(nameof(GoalDto.CompletedMilestonesCount))]
        [MapperIgnoreTarget(nameof(GoalDto.MilestoneCompletionRate))]
        public override partial void Map(Goal source, GoalDto destination);

        public override void AfterMap(Goal source, GoalDto destination)
        {
            destination.ImportanceScore = source.Importance.Value;
            destination.UrgencyScore = source.Urgency.Value;
            destination.EisenhowerQuadrant = source.GetEisenhowerQuadrant();
            destination.IsOverdue = source.IsOverdue;
            destination.DaysRemaining = source.GetDaysRemaining();
            destination.ProgressLevel = source.GetProgressLevel().ToString();
            destination.CompletedMilestonesCount = source.GetCompletedMilestonesCount();
            destination.MilestoneCompletionRate = source.GetMilestoneCompletionRate();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class GoalMilestoneToGoalMilestoneDtoMapper : MapperBase<GoalMilestone, GoalMilestoneDto>
    {
        [MapperIgnoreTarget(nameof(GoalMilestoneDto.IsOverdue))]
        public override partial GoalMilestoneDto Map(GoalMilestone source);

        [MapperIgnoreTarget(nameof(GoalMilestoneDto.IsOverdue))]
        public override partial void Map(GoalMilestone source, GoalMilestoneDto destination);

        public override void AfterMap(GoalMilestone source, GoalMilestoneDto destination)
        {
            destination.IsOverdue = source.IsOverdue;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AchievementToAchievementDtoMapper : MapperBase<Achievement, AchievementDto>
    {
        public override partial AchievementDto Map(Achievement source);
        public override partial void Map(Achievement source, AchievementDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class UserAchievementToUserAchievementDtoMapper : MapperBase<UserAchievement, UserAchievementDto>
    {
        [MapperIgnoreTarget(nameof(UserAchievementDto.IsFullyUnlocked))]
        [MapperIgnoreTarget(nameof(UserAchievementDto.DaysSinceUnlock))]
        public override partial UserAchievementDto Map(UserAchievement source);

        [MapperIgnoreTarget(nameof(UserAchievementDto.IsFullyUnlocked))]
        [MapperIgnoreTarget(nameof(UserAchievementDto.DaysSinceUnlock))]
        public override partial void Map(UserAchievement source, UserAchievementDto destination);

        public override void AfterMap(UserAchievement source, UserAchievementDto destination)
        {
            destination.IsFullyUnlocked = source.IsFullyUnlocked;
            destination.DaysSinceUnlock = source.DaysSinceUnlock;
        }
    }
}
