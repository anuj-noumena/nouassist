using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskNotesUpdated")]
    public class TaskNotesUpdatedEto : IMultiTenant
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public string OldNotes { get; set; }
        public string NewNotes { get; set; }
        public int Version { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
