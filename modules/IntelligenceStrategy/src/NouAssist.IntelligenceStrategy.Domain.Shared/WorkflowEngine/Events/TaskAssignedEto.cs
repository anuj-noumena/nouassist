using System;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Events
{
    /// <summary>
    /// Event published when a task is assigned
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.WorkflowEngine.TaskAssigned")]
    public class TaskAssignedEto : WorkflowEventBase
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
        /// The type of the assignee (User, Team, Role)
        /// </summary>
        public AssigneeType AssigneeType { get; set; }
        
        /// <summary>
        /// The ID of the assignee
        /// </summary>
        public Guid AssigneeId { get; set; }
        
        /// <summary>
        /// The timestamp when the task was assigned
        /// </summary>
        public DateTime AssignedTime { get; set; }
        
        /// <summary>
        /// The due date for the task, if any
        /// </summary>
        public DateTime? DueDate { get; set; }
    }
}
