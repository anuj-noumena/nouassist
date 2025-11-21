using System;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Events
{
    /// <summary>
    /// Event published when a workflow instance is completed
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.WorkflowEngine.WorkflowInstanceCompleted")]
    public class WorkflowInstanceCompletedEto : WorkflowEventBase
    {
        /// <summary>
        /// The ID of the workflow definition
        /// </summary>
        public Guid WorkflowDefinitionId { get; set; }
        
        /// <summary>
        /// The name of the workflow
        /// </summary>
        public string WorkflowName { get; set; }
        
        /// <summary>
        /// The timestamp when the workflow instance started
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// The timestamp when the workflow instance completed
        /// </summary>
        public DateTime CompletionTime { get; set; }
        
        /// <summary>
        /// The duration of the workflow instance in minutes
        /// </summary>
        public double DurationMinutes { get; set; }
    }
}
