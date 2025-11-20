using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for updating a project milestone's status
    /// </summary>
    public class UpdateProjectMilestoneStatusDto
    {
        /// <summary>
        /// Indicates if the milestone is completed
        /// </summary>
        [Required]
        public bool IsCompleted { get; set; }
    }
}
