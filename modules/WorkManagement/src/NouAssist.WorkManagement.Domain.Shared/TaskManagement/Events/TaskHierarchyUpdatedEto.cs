using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskHierarchyUpdated")]
    public class TaskHierarchyUpdatedEto : IMultiTenant
    {
        public Guid TaskHierarchyId { get; set; }
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
