using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskActionStepItemAdded")]
    public class TaskActionStepItemAddedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ActionStepId { get; set; }
        public string ActionStepTitle { get; set; }
        public bool IsRequired { get; set; }
        public int Sequence { get; set; }
        public int Version { get; set; }
        public DateTime AddedAt { get; set; }
    }
}