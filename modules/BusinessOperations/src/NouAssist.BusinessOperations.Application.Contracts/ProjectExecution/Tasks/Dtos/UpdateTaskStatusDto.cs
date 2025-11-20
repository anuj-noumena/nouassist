using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Enums;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for updating task status
    /// </summary>
    public class UpdateTaskStatusDto
    {
        /// <summary>
        /// New status of the task
        /// </summary>
        [Required]
        public TaskStatus Status { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Enums;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for updating a task's status
    /// </summary>
    public class UpdateTaskStatusDto
    {
        /// <summary>
        /// New status for the task
        /// </summary>
        [Required]
        public TaskStatus Status { get; set; }
    }
}
