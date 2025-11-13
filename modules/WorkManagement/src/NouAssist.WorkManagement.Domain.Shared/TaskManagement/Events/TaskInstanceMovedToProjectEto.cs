using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceMovedToProject")]
    public class TaskInstanceMovedToProjectEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? PreviousAreaId { get; set; }
        public DateTime MovedAt { get; set; }
    }
}