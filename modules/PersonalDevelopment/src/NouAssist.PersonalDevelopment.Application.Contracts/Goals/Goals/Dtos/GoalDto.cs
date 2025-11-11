using System;
using System.Collections.Generic;
using NouAssist.Core.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Goals.Dtos
{
    public class GoalDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? ParentGoalId { get; set; }
        public GoalStatus Status { get; set; }
        public int Progress { get; set; }
        public string? SmartDescription { get; set; }
        public string? CompletionNotes { get; set; }
        public double ImportanceScore { get; set; }
        public double UrgencyScore { get; set; }
        public EisenhowerQuadrant EisenhowerQuadrant { get; set; }
        public DateTime? TargetDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<GoalMilestoneDto> Milestones { get; set; }
        
        // Computed properties
        public bool IsOverdue { get; set; }
        public int? DaysRemaining { get; set; }
        public string ProgressLevel { get; set; }
        public int CompletedMilestonesCount { get; set; }
        public double MilestoneCompletionRate { get; set; }
    }

    public class GoalMilestoneDto : FullAuditedEntityDto<Guid>
    {
        public Guid GoalId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? TargetDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int OrderIndex { get; set; }
        public bool IsOverdue { get; set; }
    }

    public class CreateGoalDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? ParentGoalId { get; set; }
        public double? ImportanceScore { get; set; }
        public double? UrgencyScore { get; set; }
        public DateTime? TargetDate { get; set; }
        public string? SmartDescription { get; set; }
    }

    public class UpdateGoalDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? AreaId { get; set; }
        public DateTime? TargetDate { get; set; }
        public string? SmartDescription { get; set; }
        public double? ImportanceScore { get; set; }
        public double? UrgencyScore { get; set; }
    }

    public class UpdateGoalProgressDto
    {
        public int Progress { get; set; }
    }

    public class AchieveGoalDto
    {
        public string? CompletionNotes { get; set; }
    }

    public class ArchiveGoalDto
    {
        public string? ArchiveReason { get; set; }
    }

    public class AddGoalMilestoneDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? TargetDate { get; set; }
    }

    public class GetGoalListDto : PagedAndSortedResultRequestDto
    {
        public Guid? AreaId { get; set; }
        public GoalStatus? Status { get; set; }
        public bool? IsOverdue { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class GoalStatisticsDto
    {
        public int TotalGoals { get; set; }
        public int ActiveGoals { get; set; }
        public int AchievedGoals { get; set; }
        public int OnHoldGoals { get; set; }
        public int OverdueGoals { get; set; }
        public double AverageProgress { get; set; }
        public double CompletionRate { get; set; }
    }

    public class AchievementDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Icon { get; set; }
        public string? BadgeImageUrl { get; set; }
        public AchievementCategory Category { get; set; }
        public AchievementRarity Rarity { get; set; }
        public int Points { get; set; }
        public bool IsSecret { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserAchievementDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public Guid AchievementId { get; set; }
        public AchievementDto Achievement { get; set; }
        public DateTime UnlockedAt { get; set; }
        public double ProgressPercentage { get; set; }
        public bool IsFullyUnlocked { get; set; }
        public int DaysSinceUnlock { get; set; }
    }
}
