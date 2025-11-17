using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskAttachmentAdded")]
    public class TaskAttachmentAddedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public int AttachmentCount { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
