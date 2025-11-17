using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskInstanceNotesUpdated")]
    public class TaskInstanceNotesUpdatedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public string OldNotes { get; set; }
        public string NewNotes { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
