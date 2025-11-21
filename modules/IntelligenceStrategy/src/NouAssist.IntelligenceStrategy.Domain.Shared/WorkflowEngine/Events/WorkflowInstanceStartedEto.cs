using System;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Events
{
    /// <summary>
    /// Event published when a workflow instance is started
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.WorkflowEngine.WorkflowInstanceStarted")]
    public class WorkflowInstanceStartedEto : WorkflowEventBase
    {
        /// <summary>
        /// The ID of the workflow definition
        /// </summary>
        public Guid WorkflowDefinitionId { get; set; }
        
        /// <summary>
        /// The version number of the workflow definition
        /// </summary>
        public int VersionNumber { get; set; }
        
        /// <summary>
        /// The name of the workflow
        /// </summary>
        public string WorkflowName { get; set; }
        
        /// <summary>
        /// The timestamp when the workflow instance started
        /// </summary>
        public DateTime StartTime { get; set; }
    }
}
