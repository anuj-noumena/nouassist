using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceCreated")]
    public class TaskInstanceCreatedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid TaskId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
