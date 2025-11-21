using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Exceptions;
using NouAssist.IntelligenceStrategy.WorkflowEngine.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Entities
{
    /// <summary>
    /// Represents an instance of an activity within a workflow instance
    /// </summary>
    public class ActivityInstance : Entity<Guid>
    {
        /// <summary>
        /// The ID of the workflow instance this activity instance belongs to
        /// </summary>
        public Guid WorkflowInstanceId { get; private set; }
        
        /// <summary>
        /// The ID of the activity definition this instance is based on
        /// </summary>
        public Guid ActivityDefinitionId { get; private set; }
        
        /// <summary>
        /// The name of the activity
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The type of the activity
        /// </summary>
        public ActivityType ActivityType { get; private set; }
        
        /// <summary>
        /// The status of the activity instance
        /// </summary>
        public TaskStatus Status { get; private set; }
        
        /// <summary>
        /// The timestamp when the activity was started
        /// </summary>
        public DateTime? StartTime { get; private set; }
        
        /// <summary>
        /// The timestamp when the activity was completed
        /// </summary>
        public DateTime? CompletionTime { get; private set; }
        
        /// <summary>
        /// The due date for the activity, if any
        /// </summary>
        public DateTime? DueDate { get; private set; }
        
        /// <summary>
        /// The assignee of the activity, if it's a user task
        /// </summary>
        public AssigneeReference Assignee { get; private set; }
        
        /// <summary>
        /// Custom properties for the activity instance
        /// </summary>
        public Dictionary<string, string> Properties { get; private set; }

        protected ActivityInstance() { }

        internal ActivityInstance(
            Guid id,
            Guid workflowInstanceId,
            Guid activityDefinitionId,
            string name,
            ActivityType activityType)
            : base(id)
        {
            WorkflowInstanceId = workflowInstanceId;
            ActivityDefinitionId = activityDefinitionId;
            Name = name;
            ActivityType = activityType;
            Status = TaskStatus.Created;
            Properties = new Dictionary<string, string>();
        }
        
        /// <summary>
        /// Assigns the activity to a user, team, or role
        /// </summary>
        /// <param name="assignee">The assignee reference</param>
        public void Assign(AssigneeReference assignee)
        {
            if (Status != TaskStatus.Created && Status != TaskStatus.Waiting)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:InvalidTaskState",
                    $"Cannot assign task in status {Status}."
                );
            }
            
            Assignee = assignee;
            Status = TaskStatus.Assigned;
        }
        
        /// <summary>
        /// Starts the activity
        /// </summary>
        public void Start()
        {
            if (Status != TaskStatus.Assigned)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:InvalidTaskState",
                    $"Cannot start task in status {Status}."
                );
            }
            
            Status = TaskStatus.InProgress;
            StartTime = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Completes the activity
        /// </summary>
        /// <param name="properties">Optional output properties</param>
        public void Complete(Dictionary<string, string> properties = null)
        {
            if (Status != TaskStatus.InProgress)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:InvalidTaskState",
                    $"Cannot complete task in status {Status}."
                );
            }
            
            Status = TaskStatus.Completed;
            CompletionTime = DateTime.UtcNow;
            
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    Properties[property.Key] = property.Value;
                }
            }
        }
        
        /// <summary>
        /// Fails the activity
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        public void Fail(string errorMessage)
        {
            if (Status != TaskStatus.InProgress)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:InvalidTaskState",
                    $"Cannot fail task in status {Status}."
                );
            }
            
            Status = TaskStatus.Failed;
            Properties["ErrorMessage"] = errorMessage;
        }
        
        /// <summary>
        /// Cancels the activity
        /// </summary>
        /// <param name="reason">The cancellation reason</param>
        public void Cancel(string reason)
        {
            if (Status == TaskStatus.Completed || Status == TaskStatus.Failed)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:InvalidTaskState",
                    $"Cannot cancel task in status {Status}."
                );
            }
            
            Status = TaskStatus.Cancelled;
            Properties["CancellationReason"] = reason;
        }
        
        /// <summary>
        /// Sets the due date for the activity
        /// </summary>
        /// <param name="dueDate">The due date</param>
        public void SetDueDate(DateTime dueDate)
        {
            if (Status == TaskStatus.Completed || Status == TaskStatus.Failed || Status == TaskStatus.Cancelled)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:InvalidTaskState",
                    $"Cannot set due date for task in status {Status}."
                );
            }
            
            DueDate = dueDate;
        }
        
        /// <summary>
        /// Sets a property value
        /// </summary>
        /// <param name="key">The property key</param>
        /// <param name="value">The property value</param>
        public void SetProperty(string key, string value)
        {
            Properties[key] = value;
        }
    }
}
