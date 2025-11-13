using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events;

[EventName("NouAssist.Tasks.TaskContextChanged")]
public class TaskContextChangedEto : IMultiTenant
{
    public Guid TaskId { get; set; }
    public Guid? TenantId { get; set; }
    public Guid? OldContextId { get; set; }
    public Guid? NewContextId { get; set; }
    public int Version { get; set; }
    public DateTime ChangedAt { get; set; }
}