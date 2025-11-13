using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceContextChanged")]
    public class TaskInstanceContextChangedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? OldContextId { get; set; }
        public Guid? NewContextId { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}