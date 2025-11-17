using System;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [Serializable]
    public class TaskChecklistItemCheckedEto
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ChecklistItemId { get; set; }
        public Guid? CheckedBy { get; set; }
        public DateTime CheckedAt { get; set; }
        public double CompletionPercentage { get; set; }
    }
}
