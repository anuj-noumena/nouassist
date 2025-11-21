using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.ProjectExecution.Entities;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using NouAssist.BusinessOperations.ProjectExecution.Events;
using NouAssist.BusinessOperations.ProjectExecution.Exceptions;
using NouAssist.BusinessOperations.ProjectExecution.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace NouAssist.BusinessOperations.ProjectExecution.Aggregates
{
    /// <summary>
    /// Task aggregate root representing a unit of work within a project
    /// </summary>
    public class Task : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        #region Properties
        
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// ID of the project this task belongs to
        /// </summary>
        public Guid ProjectId { get; private set; }
        
        /// <summary>
        /// Optional ID of the phase this task belongs to
        /// </summary>
        public Guid? PhaseId { get; private set; }
        
        /// <summary>
        /// Task details value object
        /// </summary>
        public TaskDetails Details { get; private set; }
        
        /// <summary>
        /// Current status of the task
        /// </summary>
        public TaskStatus Status { get; private set; }
        
        /// <summary>
        /// Start date of the task, if specified
        /// </summary>
        public DateTime? StartDate { get; private set; }
        
        /// <summary>
        /// Due date of the task, if specified
        /// </summary>
        public DateTime? DueDate { get; private set; }
        
        /// <summary>
        /// Completion date of the task, if completed
        /// </summary>
        public DateTime? CompletionDate { get; private set; }
        
        /// <summary>
        /// Collection of task assignments
        /// </summary>
        private readonly List<TaskAssignment> _assignments;
        public IReadOnlyCollection<TaskAssignment> Assignments => _assignments.AsReadOnly();
        
        /// <summary>
        /// Collection of task dependencies
        /// </summary>
        private readonly List<TaskDependency> _dependencies;
        public IReadOnlyCollection<TaskDependency> Dependencies => _dependencies.AsReadOnly();
        
        #endregion
        
        #region Constructors
        
        protected Task()
        {
            _assignments = new List<TaskAssignment>();
            _dependencies = new List<TaskDependency>();
        }
        
        /// <summary>
        /// Creates a new instance of Task aggregate
        /// </summary>
        public static Task Create(
            Guid id,
            Guid projectId,
            string name,
            string description,
            double estimatedHours,
            TaskPriority priority,
            DateTime? startDate = null,
            DateTime? dueDate = null,
            Guid? phaseId = null,
            Guid? tenantId = null)
        {
            return new Task(
                id,
                projectId,
                name,
                description,
                estimatedHours,
                priority,
                startDate,
                dueDate,
                phaseId,
                tenantId);
        }
        
        private Task(
            Guid id,
            Guid projectId,
            string name,
            string description,
            double estimatedHours,
            TaskPriority priority,
            DateTime? startDate = null,
            DateTime? dueDate = null,
            Guid? phaseId = null,
            Guid? tenantId = null) : base(id)
        {
            ProjectId = projectId;
            Details = new TaskDetails(name, description, estimatedHours, priority);
            Status = TaskStatus.ToDo;
            StartDate = startDate;
            DueDate = dueDate;
            PhaseId = phaseId;
            TenantId = tenantId;
            
            _assignments = new List<TaskAssignment>();
            _dependencies = new List<TaskDependency>();
            
            ValidateDates();
            
            AddDistributedEvent(new TaskCreatedEto
            {
                Id = Id,
                TenantId = TenantId,
                ProjectId = ProjectId,
                Name = Details.Name,
                Description = Details.Description,
                EstimatedHours = Details.EstimatedHours,
                Priority = Details.Priority
            });
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Updates the task details
        /// </summary>
        public void UpdateDetails(
            string name,
            string description,
            double estimatedHours,
            TaskPriority priority)
        {
            EnsureCanBeModified();
            
            Details = new TaskDetails(name, description, estimatedHours, priority);
        }
        
        /// <summary>
        /// Updates the task dates
        /// </summary>
        public void UpdateDates(DateTime? startDate, DateTime? dueDate)
        {
            EnsureCanBeModified();
            
            StartDate = startDate;
            DueDate = dueDate;
            
            ValidateDates();
        }
        
        /// <summary>
        /// Updates the task status
        /// </summary>
        public void UpdateStatus(TaskStatus newStatus)
        {
            if (!CanTransitionTo(newStatus))
            {
                throw new InvalidTaskStatusTransitionException(Status, newStatus);
            }
            
            var previousStatus = Status;
            Status = newStatus;
            
            if (newStatus == TaskStatus.InProgress && !StartDate.HasValue)
            {
                StartDate = Clock.Now;
            }
            
            if (newStatus == TaskStatus.Done)
            {
                CompletionDate = Clock.Now;
            }
            else
            {
                CompletionDate = null;
            }
            
            AddDistributedEvent(new TaskStatusUpdatedEto
            {
                Id = Id,
                TenantId = TenantId,
                ProjectId = ProjectId,
                PreviousStatus = previousStatus,
                NewStatus = newStatus
            });
        }
        
        /// <summary>
        /// Assigns the task to a user
        /// </summary>
        public TaskAssignment AssignTo(Guid userId)
        {
            EnsureCanBeModified();
            
            if (_assignments.Any(a => a.UserId == userId && a.IsActive))
            {
                throw new BusinessException("User is already assigned to this task");
            }
            
            var assignment = new TaskAssignment(Id, userId, Clock.Now);
            _assignments.Add(assignment);
            
            return assignment;
        }
        
        /// <summary>
        /// Unassigns the task from a user
        /// </summary>
        public void UnassignFrom(Guid userId)
        {
            EnsureCanBeModified();
            
            var assignment = _assignments.FirstOrDefault(a => a.UserId == userId && a.IsActive);
            if (assignment != null)
            {
                assignment.Unassign(Clock.Now);
            }
        }
        
        /// <summary>
        /// Adds a dependency on another task
        /// </summary>
        public TaskDependency AddDependency(Guid dependsOnTaskId, DependencyType dependencyType)
        {
            EnsureCanBeModified();
            
            if (dependsOnTaskId == Id)
            {
                throw new BusinessException("Task cannot depend on itself");
            }
            
            if (_dependencies.Any(d => d.DependsOnTaskId == dependsOnTaskId && d.DependencyType == dependencyType))
            {
                throw new BusinessException("This dependency already exists");
            }
            
            var dependency = new TaskDependency(Id, dependsOnTaskId, dependencyType);
            _dependencies.Add(dependency);
            
            return dependency;
        }
        
        /// <summary>
        /// Removes a dependency
        /// </summary>
        public void RemoveDependency(Guid dependsOnTaskId, DependencyType dependencyType)
        {
            EnsureCanBeModified();
            
            var dependency = _dependencies.FirstOrDefault(d => 
                d.DependsOnTaskId == dependsOnTaskId && d.DependencyType == dependencyType);
                
            if (dependency != null)
            {
                _dependencies.Remove(dependency);
            }
        }
        
        /// <summary>
        /// Sets the phase for this task
        /// </summary>
        public void SetPhase(Guid? phaseId)
        {
            EnsureCanBeModified();
            
            PhaseId = phaseId;
        }
        
        #endregion
        
        #region Private Methods
        
        private bool CanTransitionTo(TaskStatus targetStatus)
        {
            switch (Status)
            {
                case TaskStatus.ToDo:
                    return targetStatus == TaskStatus.InProgress || targetStatus == TaskStatus.Blocked;
                
                case TaskStatus.InProgress:
                    return targetStatus == TaskStatus.ToDo || 
                           targetStatus == TaskStatus.Done || 
                           targetStatus == TaskStatus.Blocked;
                
                case TaskStatus.Done:
                    return targetStatus == TaskStatus.ToDo || targetStatus == TaskStatus.InProgress;
                
                case TaskStatus.Blocked:
                    return targetStatus == TaskStatus.ToDo || targetStatus == TaskStatus.InProgress;
                
                default:
                    return false;
            }
        }
        
        private void ValidateDates()
        {
            if (StartDate.HasValue && DueDate.HasValue && DueDate.Value < StartDate.Value)
            {
                throw new BusinessException("ProjectExecution:Validation:DateOrderInvalid");
            }
        }
        
        private void EnsureCanBeModified()
        {
            if (Status == TaskStatus.Done)
            {
                throw new BusinessException("ProjectExecution:ErrorMessage:TaskCannotBeModified")
                    .WithData("status", Status);
            }
        }
        
        #endregion
    }
}
