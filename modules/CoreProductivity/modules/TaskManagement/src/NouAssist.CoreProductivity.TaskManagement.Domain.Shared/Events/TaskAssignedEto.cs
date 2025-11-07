using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Tasks.Events
{
    [EventName("NouAssist.Tasks.TaskAssigned")]
    public class TaskAssignedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid AssignedTo { get; set; }
        public Guid AssignedBy { get; set; }
        public DateTime AssignedAt { get; set; }
    }
    
    [EventName("NouAssist.Tasks.TaskUnassigned")]
    public class TaskUnassignedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
    }
}
