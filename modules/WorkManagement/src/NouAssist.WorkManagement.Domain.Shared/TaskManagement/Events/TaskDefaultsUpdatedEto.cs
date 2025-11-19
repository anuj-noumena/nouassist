using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskDefaultsUpdated")]
    public class TaskDefaultsUpdatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid TaskDefaultsId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
