using System;
using NouAssist.WorkManagement.TaskManagement.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceComplexityEstimationUpdated")]
    public class TaskInstanceComplexityEstimationUpdatedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public int? ComplexityValue { get; set; }
        public ComplexityScale? ComplexityScale { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
