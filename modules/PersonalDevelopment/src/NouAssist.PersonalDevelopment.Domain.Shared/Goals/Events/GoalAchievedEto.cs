using System;

namespace NouAssist.PersonalDevelopment.Goals.Events
{
    [Serializable]
    public class GoalAchievedEto
    {
        public Guid GoalId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime AchievedAt { get; set; }
        public int Progress { get; set; }
        public string? CompletionNotes { get; set; }
    }
}
