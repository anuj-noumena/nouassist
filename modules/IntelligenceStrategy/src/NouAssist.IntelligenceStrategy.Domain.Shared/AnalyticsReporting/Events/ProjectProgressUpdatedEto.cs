using System;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    /// <summary>
    /// Event published when a project's progress is updated in the Project BC
    /// </summary>
    [EventName("NouAssist.Project.ProjectProgressUpdated")]
    public class ProjectProgressUpdatedEto
    {
        public Guid ProjectId { get; set; }
        
        public string ProjectName { get; set; }
        
        public decimal ProgressPercentage { get; set; }
        
        public int CompletedTasks { get; set; }
        
        public int TotalTasks { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}
