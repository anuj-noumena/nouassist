using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for Task entity with additional details
    /// </summary>
    public class TaskWithDetailsDto : TaskDto
    {
        /// <summary>
        /// Task assignments
        /// </summary>
        public List<TaskAssignmentDto> Assignments { get; set; }
        
        /// <summary>
        /// Task dependencies
        /// </summary>
        public List<TaskDependencyDto> Dependencies { get; set; }
        
        /// <summary>
        /// Creates a new instance of TaskWithDetailsDto
        /// </summary>
        public TaskWithDetailsDto()
        {
            Assignments = new List<TaskAssignmentDto>();
            Dependencies = new List<TaskDependencyDto>();
        }
    }
}
