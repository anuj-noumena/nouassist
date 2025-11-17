using System;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [Serializable]
    public class TaskDependencyAddedEto
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid DependsOnTaskInstanceId { get; set; }
        public string DependencyType { get; set; }
        public int LagMinutes { get; set; }
        public bool IsBlocking { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
