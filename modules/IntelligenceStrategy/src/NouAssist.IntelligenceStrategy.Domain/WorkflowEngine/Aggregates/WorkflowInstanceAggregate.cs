using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Entities;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Events;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Exceptions;
using NouAssist.IntelligenceStrategy.WorkflowEngine.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates
{
    /// <summary>
    /// Represents an instance of a workflow definition
    /// </summary>
    public class WorkflowInstance : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID this instance belongs to
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// The ID of the workflow definition this instance is based on
        /// </summary>
        public Guid WorkflowDefinitionId { get; private set; }
        
        /// <summary>
        /// The version number of the workflow definition
        /// </summary>
        public int VersionNumber { get; private set; }
        
        /// <summary>
        /// The name of the workflow instance
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The current status of the workflow instance
        /// </summary>
        public InstanceStatus Status { get; private set; }
        
        /// <summary>
        /// The timestamp when the workflow instance was started
        /// </summary>
        public DateTime StartTime { get; private set; }
        
        /// <summary>
        /// The timestamp when the workflow instance was completed
        /// </summary>
        public DateTime? CompletionTime { get; private set; }
        
        /// <summary>
        /// The context data for the workflow instance
        /// </summary>
        public Dictionary<string, string> ContextData { get; private set; }
        
        private readonly List<ActivityInstance> _activityInstances;
        
        /// <summary>
        /// The activity instances in this workflow instance
        /// </summary>
        public IReadOnlyCollection<ActivityInstance> ActivityInstances => _activityInstances.AsReadOnly();
        
        private readonly List<WorkflowExecutionLog> _executionLogs;
        
        /// <summary>
        /// The execution logs for this workflow instance
        /// </summary>
        public IReadOnlyCollection<WorkflowExecutionLog> ExecutionLogs => _executionLogs.AsReadOnly();

        protected WorkflowInstance()
        {
            _activityInstances = new List<ActivityInstance>();
            _executionLogs = new List<WorkflowExecutionLog>();
        }

        /// <summary>
        /// Creates a new workflow instance
        /// </summary>
        /// <param name="id">The ID of the instance</param>
        /// <param name="workflowDefinitionId">The ID of the workflow definition</param>
        /// <param name="versionNumber">The version number of the workflow definition</param>
        /// <param name="name">The name of the instance</param>
        /// <param name="tenantId">The tenant ID</param>
        /// <param name="initialContextData">Initial context data</param>
        /// <returns>A new workflow instance</returns>
        public static WorkflowInstance Create(
            Guid id,
            Guid workflowDefinitionId,
            int versionNumber,
            string name,
            Guid? tenantId = null,
            Dictionary<string, string> initialContextData = null)
        {
            return new WorkflowInstance(id, workflowDefinitionId, versionNumber, name, tenantId, initialContextData);
        }

        private WorkflowInstance(
            Guid id,
            Guid workflowDefinitionId,
            int versionNumber,
            string name,
            Guid? tenantId = null,
            Dictionary<string, string> initialContextData = null)
            : base(id)
        {
            TenantId = tenantId;
            WorkflowDefinitionId = workflowDefinitionId;
            VersionNumber = versionNumber;
            SetName(name);
            Status = InstanceStatus.Running;
            StartTime = DateTime.UtcNow;
            ContextData = initialContextData ?? new Dictionary<string, string>();
            _activityInstances = new List<ActivityInstance>();
            _executionLogs = new List<WorkflowExecutionLog>();
            
            // Add initial log entry
            AddLogEntry("WorkflowStarted", $"Workflow '{name}' started");
            
            AddDistributedEvent(new WorkflowInstanceStartedEto
            {
                Id = Id,
                WorkflowDefinitionId = WorkflowDefinitionId,
                VersionNumber = VersionNumber,
                WorkflowName = Name,
                StartTime = StartTime,
                TenantId = TenantId
            });
        }
        
        /// <summary>
        /// Adds an activity instance to this workflow instance
        /// </summary>
        /// <param name="id">The ID of the activity instance</param>
        /// <param name="activityDefinitionId">The ID of the activity definition</param>
        /// <param name="name">The name of the activity</param>
        /// <param name="activityType">The type of the activity</param>
        /// <returns>The created activity instance</returns>
        public ActivityInstance AddActivityInstance(
            Guid id,
            Guid activityDefinitionId,
            string name,
            ActivityType activityType)
        {
            EnsureWorkflowIsActive();
            
            var activityInstance = new ActivityInstance(
                id,
                Id,
                activityDefinitionId,
                name,
                activityType
            );
            
            _activityInstances.Add(activityInstance);
            
            AddLogEntry(
                "ActivityCreated",
                $"Activity '{name}' created",
                new Dictionary<string, string>
                {
                    { "ActivityInstanceId", id.ToString() },
                    { "ActivityType", activityType.ToString() }
                }
            );
            
            return activityInstance;
        }
        
        /// <summary>
        /// Gets a specific activity instance
        /// </summary>
        /// <param name="activityInstanceId">The ID of the activity instance</param>
        /// <returns>The activity instance</returns>
        public ActivityInstance GetActivityInstance(Guid activityInstanceId)
        {
            var activityInstance = _activityInstances.FirstOrDefault(a => a.Id == activityInstanceId);
            if (activityInstance == null)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:ActivityInstanceNotFound",
                    $"Activity instance {activityInstanceId} not found."
                );
            }
            
            return activityInstance;
        }
        
        /// <summary>
        /// Assigns a task to a user, team, or role
        /// </summary>
        /// <param name="activityInstanceId">The ID of the activity instance</param>
        /// <param name="assignee">The assignee reference</param>
        /// <param name="dueDate">Optional due date</param>
        public void AssignTask(
            Guid activityInstanceId,
            AssigneeReference assignee,
            DateTime? dueDate = null)
        {
            EnsureWorkflowIsActive();
            
            var activityInstance = GetActivityInstance(activityInstanceId);
            
            activityInstance.Assign(assignee);
            
            if (dueDate.HasValue)
            {
                activityInstance.SetDueDate(dueDate.Value);
            }
            
            AddLogEntry(
                "TaskAssigned",
                $"Task '{activityInstance.Name}' assigned to {assignee.Type} {assignee.Id}",
                new Dictionary<string, string>
                {
                    { "ActivityInstanceId", activityInstanceId.ToString() },
                    { "AssigneeType", assignee.Type.ToString() },
                    { "AssigneeId", assignee.Id.ToString() },
                    { "DueDate", dueDate?.ToString("o") }
                }
            );
            
            AddDistributedEvent(new TaskAssignedEto
            {
                Id = Guid.NewGuid(),
                WorkflowInstanceId = Id,
                ActivityInstanceId = activityInstanceId,
                AssigneeType = assignee.Type,
                AssigneeId = assignee.Id,
                AssignedTime = DateTime.UtcNow,
                DueDate = dueDate,
                TenantId = TenantId
            });
        }
        
        /// <summary>
        /// Starts an activity
        /// </summary>
        /// <param name="activityInstanceId">The ID of the activity instance</param>
        public void StartActivity(Guid activityInstanceId)
        {
            EnsureWorkflowIsActive();
            
            var activityInstance = GetActivityInstance(activityInstanceId);
            
            activityInstance.Start();
            
            AddLogEntry(
                "ActivityStarted",
                $"Activity '{activityInstance.Name}' started",
                new Dictionary<string, string>
                {
                    { "ActivityInstanceId", activityInstanceId.ToString() }
                }
            );
        }
        
        /// <summary>
        /// Completes an activity
        /// </summary>
        /// <param name="activityInstanceId">The ID of the activity instance</param>
        /// <param name="outputData">Optional output data</param>
        public void CompleteActivity(
            Guid activityInstanceId,
            Dictionary<string, string> outputData = null)
        {
            EnsureWorkflowIsActive();
            
            var activityInstance = GetActivityInstance(activityInstanceId);
            
            activityInstance.Complete(outputData);
            
            // If output data is provided, store it in the context
            if (outputData != null)
            {
                foreach (var item in outputData)
                {
                    ContextData[$"{activityInstance.Name}.{item.Key}"] = item.Value;
                }
            }
            
            AddLogEntry(
                "ActivityCompleted",
                $"Activity '{activityInstance.Name}' completed",
                new Dictionary<string, string>
                {
                    { "ActivityInstanceId", activityInstanceId.ToString() }
                }
            );
            
            AddDistributedEvent(new WorkflowActivityCompletedEto
            {
                Id = Guid.NewGuid(),
                WorkflowInstanceId = Id,
                ActivityInstanceId = activityInstanceId,
                ActivityName = activityInstance.Name,
                ActivityType = activityInstance.ActivityType,
                CompletionTime = DateTime.UtcNow,
                TenantId = TenantId
            });
            
            // Check if all activities are completed to potentially complete the workflow
            CheckForWorkflowCompletion();
        }
        
        /// <summary>
        /// Fails an activity
        /// </summary>
        /// <param name="activityInstanceId">The ID of the activity instance</param>
        /// <param name="errorMessage">The error message</param>
        public void FailActivity(Guid activityInstanceId, string errorMessage)
        {
            EnsureWorkflowIsActive();
            
            var activityInstance = GetActivityInstance(activityInstanceId);
            
            activityInstance.Fail(errorMessage);
            
            AddLogEntry(
                "ActivityFailed",
                $"Activity '{activityInstance.Name}' failed: {errorMessage}",
                new Dictionary<string, string>
                {
                    { "ActivityInstanceId", activityInstanceId.ToString() },
                    { "ErrorMessage", errorMessage }
                }
            );
            
            // If a critical activity fails, mark the workflow as failed
            if (activityInstance.ActivityType == ActivityType.SystemTask ||
                activityInstance.ActivityType == ActivityType.ServiceTask ||
                activityInstance.ActivityType == ActivityType.ScriptTask)
            {
                FailWorkflow($"Critical activity '{activityInstance.Name}' failed: {errorMessage}");
            }
        }
        
        /// <summary>
        /// Pauses the workflow instance
        /// </summary>
        /// <param name="reason">The reason for pausing</param>
        public void Pause(string reason)
        {
            EnsureWorkflowIsActive();
            
            Status = InstanceStatus.Paused;
            
            AddLogEntry(
                "WorkflowPaused",
                $"Workflow paused: {reason}",
                new Dictionary<string, string>
                {
                    { "Reason", reason }
                }
            );
            
            AddDistributedEvent(new WorkflowInstancePausedEto
            {
                Id = Id,
                WorkflowDefinitionId = WorkflowDefinitionId,
                WorkflowName = Name,
                PauseTime = DateTime.UtcNow,
                Reason = reason,
                TenantId = TenantId
            });
        }
        
        /// <summary>
        /// Resumes the workflow instance
        /// </summary>
        public void Resume()
        {
            if (Status != InstanceStatus.Paused)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:InvalidWorkflowState",
                    $"Cannot resume workflow in status {Status}."
                );
            }
            
            Status = InstanceStatus.Running;
            
            AddLogEntry("WorkflowResumed", "Workflow resumed");
            
            AddDistributedEvent(new WorkflowInstanceResumedEto
            {
                Id = Id,
                WorkflowDefinitionId = WorkflowDefinitionId,
                WorkflowName = Name,
                ResumeTime = DateTime.UtcNow,
                TenantId = TenantId
            });
        }
        
        /// <summary>
        /// Completes the workflow instance
        /// </summary>
        public void Complete()
        {
            if (Status != InstanceStatus.Running)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:InvalidWorkflowState",
                    $"Cannot complete workflow in status {Status}."
                );
            }
            
            Status = InstanceStatus.Completed;
            CompletionTime = DateTime.UtcNow;
            
            AddLogEntry("WorkflowCompleted", "Workflow completed successfully");
            
            AddDistributedEvent(new WorkflowInstanceCompletedEto
            {
                Id = Id,
                WorkflowDefinitionId = WorkflowDefinitionId,
                WorkflowName = Name,
                StartTime = StartTime,
                CompletionTime = CompletionTime.Value,
                DurationMinutes = (CompletionTime.Value - StartTime).TotalMinutes,
                TenantId = TenantId
            });
        }
        
        /// <summary>
        /// Fails the workflow instance
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        public void FailWorkflow(string errorMessage)
        {
            if (Status != InstanceStatus.Running && Status != InstanceStatus.Paused)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:InvalidWorkflowState",
                    $"Cannot fail workflow in status {Status}."
                );
            }
            
            Status = InstanceStatus.Failed;
            CompletionTime = DateTime.UtcNow;
            
            AddLogEntry(
                "WorkflowFailed",
                $"Workflow failed: {errorMessage}",
                new Dictionary<string, string>
                {
                    { "ErrorMessage", errorMessage }
                }
            );
            
            AddDistributedEvent(new WorkflowInstanceFailedEto
            {
                Id = Id,
                WorkflowDefinitionId = WorkflowDefinitionId,
                WorkflowName = Name,
                FailureTime = DateTime.UtcNow,
                ErrorMessage = errorMessage,
                TenantId = TenantId
            });
        }
        
        /// <summary>
        /// Adds a log entry to the workflow instance
        /// </summary>
        /// <param name="logType">The type of log entry</param>
        /// <param name="message">The log message</param>
        /// <param name="data">Additional data</param>
        public void AddLogEntry(
            string logType,
            string message,
            Dictionary<string, string> data = null)
        {
            var log = new WorkflowExecutionLog(
                Guid.NewGuid(),
                Id,
                null,
                logType,
                message,
                data
            );
            
            _executionLogs.Add(log);
        }
        
        /// <summary>
        /// Sets a context value
        /// </summary>
        /// <param name="key">The context key</param>
        /// <param name="value">The context value</param>
        public void SetContextValue(string key, string value)
        {
            ContextData[key] = value;
        }
        
        /// <summary>
        /// Gets a context value
        /// </summary>
        /// <param name="key">The context key</param>
        /// <returns>The context value if found, null otherwise</returns>
        public string GetContextValue(string key)
        {
            if (ContextData.TryGetValue(key, out var value))
            {
                return value;
            }
            
            return null;
        }
        
        /// <summary>
        /// Checks if all activities are completed and completes the workflow if so
        /// </summary>
        private void CheckForWorkflowCompletion()
        {
            // If there are no activity instances, don't complete
            if (!_activityInstances.Any())
            {
                return;
            }
            
            // If there are any activities that are not completed or failed, don't complete
            if (_activityInstances.Any(a => 
                a.Status != TaskStatus.Completed && 
                a.Status != TaskStatus.Failed &&
                a.Status != TaskStatus.Cancelled))
            {
                return;
            }
            
            // If there are any end events, at least one should be completed
            var endEvents = _activityInstances.Where(a => a.ActivityType == ActivityType.EndEvent).ToList();
            if (endEvents.Any() && !endEvents.Any(e => e.Status == TaskStatus.Completed))
            {
                return;
            }
            
            // All activities are completed or failed, and any required end events are completed
            Complete();
        }
        
        /// <summary>
        /// Ensures the workflow is active and operations can be performed
        /// </summary>
        private void EnsureWorkflowIsActive()
        {
            if (Status != InstanceStatus.Running)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:InvalidWorkflowState"
                )
                .WithData("workflowId", Id)
                .WithData("currentState", Status.ToString())
                .WithData("expectedState", InstanceStatus.Running.ToString());
            }
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: WorkflowEngineConsts.MaxNameLength
            );
        }
    }
}
