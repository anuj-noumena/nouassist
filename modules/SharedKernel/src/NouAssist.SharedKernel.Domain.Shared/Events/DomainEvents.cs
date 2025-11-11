using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.SharedKernel.Events
{
    [EventName("NouAssist.Goals.Achieved")]
    public class GoalAchievedEto : IMultiTenant
    {
        public Guid GoalId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime AchievedAt { get; set; }
        public int Progress { get; set; }
        public string? CompletionNotes { get; set; }
    }

    [EventName("NouAssist.Goals.ProgressUpdated")]
    public class GoalProgressUpdatedEto : IMultiTenant
    {
        public Guid GoalId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public int OldProgress { get; set; }
        public int NewProgress { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    [EventName("NouAssist.Achievements.Unlocked")]
    public class AchievementUnlockedEto : IMultiTenant
    {
        public Guid UserAchievementId { get; set; }
        public Guid UserId { get; set; }
        public Guid AchievementId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime UnlockedAt { get; set; }
        public int Points { get; set; }
    }

    [EventName("NouAssist.Inbox.ItemProcessed")]
    public class InboxItemProcessedEto : IMultiTenant
    {
        public Guid InboxItemId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string? ProcessedToEntityType { get; set; }
        public Guid? ProcessedToEntityId { get; set; }
    }
}
