using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskCreated")]
    public class TaskCreatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public string Title { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
