using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events;

[EventName("NouAssist.Tasks.TaskAreaChanged")]
public class TaskAreaChangedEto : IMultiTenant
{
    public Guid TaskId { get; set; }
    public Guid? TenantId { get; set; }
    public Guid? OldAreaId { get; set; }
    public Guid? NewAreaId { get; set; }
    public int Version { get; set; }
    public DateTime ChangedAt { get; set; }
}