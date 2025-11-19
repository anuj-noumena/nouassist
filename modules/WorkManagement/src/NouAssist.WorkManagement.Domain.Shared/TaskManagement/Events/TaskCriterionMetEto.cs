using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskCriterionMet")]
    public class TaskCriterionMetEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid CriterionId { get; set; }
        public Guid? MetBy { get; set; }
        public DateTime MetAt { get; set; }
    }
}
