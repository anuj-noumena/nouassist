using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceAreaChanged")]
    public class TaskInstanceAreaChangedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? OldAreaId { get; set; }
        public Guid? NewAreaId { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}