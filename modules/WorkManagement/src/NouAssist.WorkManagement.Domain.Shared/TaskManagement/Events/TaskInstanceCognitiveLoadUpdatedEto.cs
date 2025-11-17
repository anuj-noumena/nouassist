using System;
using NouAssist.WorkManagement.TaskManagement.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceCognitiveLoadUpdated")]
    public class TaskInstanceCognitiveLoadUpdatedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public CognitiveLoad? CognitiveLoad { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
