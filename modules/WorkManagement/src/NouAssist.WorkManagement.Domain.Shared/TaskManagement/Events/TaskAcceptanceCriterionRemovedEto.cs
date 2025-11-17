using System;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [Serializable]
    public class TaskAcceptanceCriterionRemovedEto
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid CriterionId { get; set; }
        public int Version { get; set; }
        public DateTime RemovedAt { get; set; }
    }
}
