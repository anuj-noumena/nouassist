using System;

namespace NouAssist.PersonalDevelopment.Goals.Events
{
    [Serializable]
    public class GoalProgressUpdatedEto
    {
        public Guid GoalId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public int OldProgress { get; set; }
        public int NewProgress { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
