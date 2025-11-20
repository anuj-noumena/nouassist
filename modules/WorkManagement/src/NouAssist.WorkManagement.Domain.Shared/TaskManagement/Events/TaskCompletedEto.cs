using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [EventName("NouAssist.Tasks.TaskCompleted")]
    public class TaskCompletedEto : IMultiTenant
    {
        public Guid TaskInstanceId { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public string Title { get; set; }
        public string Resolution { get; set; }
        public string ResolutionNote { get; set; }
        public Guid? CompletedBy { get; set; }
        public DateTime CompletedAt { get; set; }
        public int ActualEffortMinutes { get; set; }

        // Custom fields for linking to learning paths
        public string LearningPathId { get; set; }
        public string Phase { get; set; }
        public bool IsTheoryTask { get; set; }
    }
}
