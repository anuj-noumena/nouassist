using System;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [Serializable]
    public class TaskDependencyRemovedEto
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid DependsOnTaskInstanceId { get; set; }
        public DateTime RemovedAt { get; set; }
    }
}
