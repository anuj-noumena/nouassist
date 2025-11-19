using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskMovedToArea")]
    public class TaskMovedToAreaEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid AreaId { get; set; }
        public Guid? PreviousAreaId { get; set; }
        public DateTime MovedAt { get; set; }
    }
}
