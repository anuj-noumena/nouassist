using System;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    /// <summary>
    /// Event published when a task status is updated in the Project BC
    /// </summary>
    [EventName("NouAssist.Project.TaskStatusUpdated")]
    public class TaskStatusUpdatedEto
    {
        public Guid TaskId { get; set; }
        
        public Guid ProjectId { get; set; }
        
        public string TaskName { get; set; }
        
        public string Status { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public Guid? UpdatedBy { get; set; }
    }
}
