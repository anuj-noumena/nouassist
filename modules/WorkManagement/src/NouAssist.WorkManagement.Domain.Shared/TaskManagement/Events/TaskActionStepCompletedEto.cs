using System;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [Serializable]
    public class TaskActionStepCompletedEto
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ActionStepId { get; set; }
        public Guid CompletedBy { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
