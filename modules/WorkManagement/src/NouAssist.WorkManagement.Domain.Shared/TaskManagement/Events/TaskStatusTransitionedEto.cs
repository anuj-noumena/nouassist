// TaskStatusTransitionedEto.cs

using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskStatusTransitioned")]
    public class TaskStatusTransitionedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid OldStatusId { get; set; }
        public Guid NewStatusId { get; set; }
        public string Comment { get; set; }
        public DateTime TransitionedAt { get; set; }
    }
}