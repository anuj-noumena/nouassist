using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskCommentAdded")]
    public class TaskCommentAddedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public int CommentCount { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
