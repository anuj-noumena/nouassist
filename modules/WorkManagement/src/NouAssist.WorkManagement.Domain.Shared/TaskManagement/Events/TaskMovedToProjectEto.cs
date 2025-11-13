using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events;

[EventName("NouAssist.Tasks.TaskMovedToProject")]
public class TaskMovedToProjectEto : IMultiTenant
{
    public Guid TaskId { get; set; }
    public Guid? TenantId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? PreviousAreaId { get; set; }
    public int Version { get; set; }
    public DateTime MovedAt { get; set; }
}