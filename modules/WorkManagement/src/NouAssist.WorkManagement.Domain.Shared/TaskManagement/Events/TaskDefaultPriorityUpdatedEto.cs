using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskDefaultPriorityUpdated")]
    public class TaskDefaultPriorityUpdatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public double OldImportance { get; set; }
        public double NewImportance { get; set; }
        public double OldUrgency { get; set; }
        public double NewUrgency { get; set; }
        public int Version { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
