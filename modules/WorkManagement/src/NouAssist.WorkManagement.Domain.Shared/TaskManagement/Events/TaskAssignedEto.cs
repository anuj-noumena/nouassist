using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskAssigned")]
    public class TaskAssignedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid AssignedTo { get; set; }
        public Guid? PreviousAssignee { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
