using System;

namespace NouAssist.TimeManagement.Calendar.Events
{
    public class TimeEntryStartedForLearningEto
    {
        public Guid TimeEntryId { get; set; }
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public string Phase { get; set; }
        public DateTime StartedAt { get; set; }
    }
}
