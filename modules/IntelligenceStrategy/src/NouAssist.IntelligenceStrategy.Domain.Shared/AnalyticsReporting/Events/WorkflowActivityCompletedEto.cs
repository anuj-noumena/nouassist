using System;
using Volo.Abp.EventBus;

namespace NouAssist.Analytics.Events
{
    /// <summary>
    /// Event published when a workflow activity is completed in the Workflow BC
    /// </summary>
    [EventName("NouAssist.Workflow.WorkflowActivityCompleted")]
    public class WorkflowActivityCompletedEto
    {
        public Guid ActivityId { get; set; }
        
        public string ActivityName { get; set; }
        
        public string ActivityType { get; set; }
        
        public Guid WorkflowId { get; set; }
        
        public DateTime StartedAt { get; set; }
        
        public DateTime CompletedAt { get; set; }
        
        public int DurationInSeconds { get; set; }
        
        public Guid? AssignedTo { get; set; }
    }
}
