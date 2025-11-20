using System;
using System.Collections.Generic;

namespace NouAssist.TimeManagement.Calendar.Events
{
    public class LearningTimeblocksCreatedEto
    {
        public Guid LearningPathId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public List<Guid> TimeblockIds { get; set; } = new();
        public int TotalScheduledMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
