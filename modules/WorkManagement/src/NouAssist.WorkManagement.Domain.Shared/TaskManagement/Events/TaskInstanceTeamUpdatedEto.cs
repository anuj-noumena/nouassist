using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceTeamUpdated")]
    public class TaskInstanceTeamUpdatedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? OldTeamId { get; set; }
        public Guid? NewTeamId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
