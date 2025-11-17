using System;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [Serializable]
    public class TaskActionStepUncompletedEto
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ActionStepId { get; set; }
        public DateTime UncompletedAt { get; set; }
    }
}
