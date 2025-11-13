using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskChecklistItemAdded")]
    public class TaskChecklistItemAddedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ChecklistItemId { get; set; }
        public string ItemTitle { get; set; }
        public bool IsRequired { get; set; }
        public int OrderIndex { get; set; }
        public int Version { get; set; }
        public DateTime AddedAt { get; set; }
    }
}