using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events;

[EventName("NouAssist.Tasks.TaskActivated")]
public class TaskActivatedEto : IMultiTenant
{
    public Guid TaskId { get; set; }
    public Guid? TenantId { get; set; }
    public DateTime ActivatedAt { get; set; }
}