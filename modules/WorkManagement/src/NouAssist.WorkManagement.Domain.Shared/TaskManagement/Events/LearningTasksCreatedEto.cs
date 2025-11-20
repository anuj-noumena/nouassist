using System;
using System.Collections.Generic;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    public class LearningTasksCreatedEto
    {
        public Guid LearningPathId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public List<Guid> TaskInstanceIds { get; set; } = new();
        public int TheoryTaskCount { get; set; }
        public int PracticeTaskCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
