using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for Task entity
    /// </summary>
    public class TaskDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// ID of the project this task belongs to
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Optional ID of the phase this task belongs to
        /// </summary>
        public Guid? PhaseId { get; set; }
        
        /// <summary>
        /// Name of the task
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the task
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Estimated hours for task completion
        /// </summary>
        public double EstimatedHours { get; set; }
        
        /// <summary>
        /// Priority of the task
        /// </summary>
        public TaskPriority Priority { get; set; }
        
        /// <summary>
        /// Current status of the task
        /// </summary>
        public TaskStatus Status { get; set; }
        
        /// <summary>
        /// Start date of the task, if specified
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// Due date of the task, if specified
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// Completion date of the task, if completed
        /// </summary>
        public DateTime? CompletionDate { get; set; }
        
        /// <summary>
        /// Collection of task assignments
        /// </summary>
        public List<TaskAssignmentDto> Assignments { get; set; }
        
        /// <summary>
        /// Collection of task dependencies
        /// </summary>
        public List<TaskDependencyDto> Dependencies { get; set; }
        
        /// <summary>
        /// Total hours logged for this task
        /// </summary>
        public double TotalLoggedHours { get; set; }
    }
}
using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for Task entity
    /// </summary>
    public class TaskDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// ID of the project this task belongs to
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Optional ID of the phase this task belongs to
        /// </summary>
        public Guid? PhaseId { get; set; }
        
        /// <summary>
        /// Name of the task
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the task
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Estimated hours for task completion
        /// </summary>
        public double EstimatedHours { get; set; }
        
        /// <summary>
        /// Priority of the task
        /// </summary>
        public TaskPriority Priority { get; set; }
        
        /// <summary>
        /// String representation of the task priority
        /// </summary>
        public string PriorityString => Priority.ToString();
        
        /// <summary>
        /// Status of the task
        /// </summary>
        public System.Threading.Tasks.TaskStatus Status { get; set; }
        
        /// <summary>
        /// String representation of the task status
        /// </summary>
        public string StatusString => Status.ToString();
        
        /// <summary>
        /// Start date of the task, if specified
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// Due date of the task, if specified
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// Completion date of the task, if completed
        /// </summary>
        public DateTime? CompletionDate { get; set; }
        
        /// <summary>
        /// Total logged hours for the task
        /// </summary>
        public double TotalLoggedHours { get; set; }
        
        /// <summary>
        /// User IDs of current assignees
        /// </summary>
        public List<Guid> AssigneeIds { get; set; } = new List<Guid>();
        
        /// <summary>
        /// Names of current assignees
        /// </summary>
        public List<string> AssigneeNames { get; set; } = new List<string>();
    }
}
