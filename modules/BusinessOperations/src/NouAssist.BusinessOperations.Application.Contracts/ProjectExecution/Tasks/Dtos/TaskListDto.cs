using System;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for task list items
    /// </summary>
    public class TaskListDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// ID of the project this task belongs to
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Name of the project (for display)
        /// </summary>
        public string ProjectName { get; set; }
        
        /// <summary>
        /// Optional ID of the phase this task belongs to
        /// </summary>
        public Guid? PhaseId { get; set; }
        
        /// <summary>
        /// Name of the phase (for display)
        /// </summary>
        public string PhaseName { get; set; }
        
        /// <summary>
        /// Name of the task
        /// </summary>
        public string Name { get; set; }
        
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
        /// Due date of the task, if specified
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// Total hours logged for this task
        /// </summary>
        public double TotalLoggedHours { get; set; }
        
        /// <summary>
        /// Number of active assignments for this task
        /// </summary>
        public int AssigneeCount { get; set; }
        
        /// <summary>
        /// Flag indicating if the task is overdue
        /// </summary>
        public bool IsOverdue { get; set; }
    }
}
