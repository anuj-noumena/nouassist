using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for assigning a task to a user
    /// </summary>
    public class AssignTaskDto
    {
        /// <summary>
        /// ID of the user to assign the task to
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
    }
}
