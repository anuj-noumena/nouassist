using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Enums;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for creating a new task dependency
    /// </summary>
    public class CreateTaskDependencyDto
    {
        /// <summary>
        /// ID of the task that is depended on
        /// </summary>
        [Required]
        public Guid DependsOnTaskId { get; set; }
        
        /// <summary>
        /// Type of dependency
        /// </summary>
        [Required]
        public DependencyType DependencyType { get; set; }
    }
}
