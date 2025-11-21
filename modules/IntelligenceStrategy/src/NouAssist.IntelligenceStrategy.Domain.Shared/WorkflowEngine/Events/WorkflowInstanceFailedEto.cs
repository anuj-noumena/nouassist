using System;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Events
{
    /// <summary>
    /// Event published when a workflow instance fails
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.WorkflowEngine.WorkflowInstanceFailed")]
    public class WorkflowInstanceFailedEto : WorkflowEventBase
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
        /// The timestamp when the workflow instance failed
        /// </summary>
        public DateTime FailureTime { get; set; }
        
        /// <summary>
        /// The error message
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
