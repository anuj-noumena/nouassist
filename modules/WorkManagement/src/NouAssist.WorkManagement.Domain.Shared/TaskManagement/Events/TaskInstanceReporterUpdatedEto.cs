using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceReporterUpdated")]
    public class TaskInstanceReporterUpdatedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? OldReporterId { get; set; }
        public Guid? NewReporterId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
