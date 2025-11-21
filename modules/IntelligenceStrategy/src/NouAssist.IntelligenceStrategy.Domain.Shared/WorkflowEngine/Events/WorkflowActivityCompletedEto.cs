using System;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Events
{
    /// <summary>
    /// Event published when a workflow activity is completed
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.WorkflowEngine.WorkflowActivityCompleted")]
    public class WorkflowActivityCompletedEto : WorkflowEventBase
    {
        /// <summary>
        /// The ID of the workflow instance
        /// </summary>
        public Guid WorkflowInstanceId { get; set; }
        
        /// <summary>
        /// The ID of the activity instance
        /// </summary>
        public Guid ActivityInstanceId { get; set; }
        
        /// <summary>
        /// The name of the activity
        /// </summary>
        public string ActivityName { get; set; }
        
        /// <summary>
        /// The type of the activity
        /// </summary>
        public ActivityType ActivityType { get; set; }
        
        /// <summary>
        /// The timestamp when the activity was completed
        /// </summary>
        public DateTime CompletionTime { get; set; }
    }
}
