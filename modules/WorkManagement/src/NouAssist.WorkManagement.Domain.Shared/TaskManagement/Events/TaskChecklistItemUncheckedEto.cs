using System;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [Serializable]
    public class TaskChecklistItemUncheckedEto
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ChecklistItemId { get; set; }
        public DateTime UncheckedAt { get; set; }
        public double CompletionPercentage { get; set; }
    }
}
