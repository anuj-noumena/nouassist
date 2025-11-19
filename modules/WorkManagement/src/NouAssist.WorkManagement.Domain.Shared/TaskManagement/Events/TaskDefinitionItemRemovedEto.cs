using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskDefinitionItemRemoved")]
    public class TaskDefinitionItemRemovedEto : IMultiTenant
    {
        public Guid TaskDefinitionId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid ItemId { get; set; }
        public string Type { get; set; } // "Checklist", "ActionStep", "AcceptanceCriterion"
        public DateTime RemovedAt { get; set; }
    }
}
