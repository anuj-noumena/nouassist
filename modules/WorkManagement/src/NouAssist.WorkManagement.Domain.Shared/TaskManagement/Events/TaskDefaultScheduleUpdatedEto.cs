using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskDefaultScheduleUpdated")]
    public class TaskDefaultScheduleUpdatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime? OldDefaultDueDate { get; set; }
        public DateTime? NewDefaultDueDate { get; set; }
        public int? OldDefaultEstimateMinutes { get; set; }
        public int? NewDefaultEstimateMinutes { get; set; }
        public int Version { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
