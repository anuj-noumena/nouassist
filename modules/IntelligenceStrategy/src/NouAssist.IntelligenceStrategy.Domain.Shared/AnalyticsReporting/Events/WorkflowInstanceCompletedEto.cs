using System;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events
{
    /// <summary>
    /// Event published when a workflow instance is completed in the Workflow BC
    /// </summary>
    [EventName("NouAssist.Workflow.WorkflowInstanceCompleted")]
    public class WorkflowInstanceCompletedEto
    {
        public Guid WorkflowId { get; set; }
        
        public string WorkflowName { get; set; }
        
        public string WorkflowType { get; set; }
        
        public DateTime StartedAt { get; set; }
        
        public DateTime CompletedAt { get; set; }
        
        public int DurationInSeconds { get; set; }
        
        public string Status { get; set; }
    }
}
