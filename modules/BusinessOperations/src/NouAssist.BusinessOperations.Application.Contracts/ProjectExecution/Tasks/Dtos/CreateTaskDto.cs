using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Enums;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for creating a new task
    /// </summary>
    public class CreateTaskDto
    {
        /// <summary>
        /// ID of the project this task belongs to
        /// </summary>
        [Required]
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Optional ID of the phase this task belongs to
        /// </summary>
        public Guid? PhaseId { get; set; }
        
        /// <summary>
        /// Name of the task
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxTaskNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the task
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxTaskDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Estimated hours for task completion
        /// </summary>
        [Required]
        [Range(0.1, 1000)]
        public double EstimatedHours { get; set; }
        
        /// <summary>
        /// Priority of the task
        /// </summary>
        [Required]
        public TaskPriority Priority { get; set; }
        
        /// <summary>
        /// Start date of the task, if specified
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// Due date of the task, if specified
        /// </summary>
        public DateTime? DueDate { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Constants;
using NouAssist.BusinessOperations.ProjectExecution.Enums;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for creating a new task
    /// </summary>
    public class CreateTaskDto
    {
        /// <summary>
        /// ID of the project this task belongs to
        /// </summary>
        [Required]
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Optional ID of the phase this task belongs to
        /// </summary>
        public Guid? PhaseId { get; set; }
        
        /// <summary>
        /// Name of the task
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxTaskNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the task
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxTaskDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Estimated hours for task completion
        /// </summary>
        [Required]
        [Range(0.1, 1000)]
        public double EstimatedHours { get; set; }
        
        /// <summary>
        /// Priority of the task
        /// </summary>
        [Required]
        public TaskPriority Priority { get; set; }
        
        /// <summary>
        /// Start date of the task, if specified
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// Due date of the task, if specified
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// Initial assignee IDs for the task
        /// </summary>
        public List<Guid> AssigneeIds { get; set; } = new List<Guid>();
    }
}
