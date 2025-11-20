using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Enums;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for updating project status
    /// </summary>
    public class UpdateProjectStatusDto
    {
        /// <summary>
        /// New status of the project
        /// </summary>
        [Required]
        public ProjectStatus Status { get; set; }
        
        /// <summary>
        /// Actual end date if the project is being completed
        /// </summary>
        public DateTime? ActualEndDate { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Enums;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for updating a project's status
    /// </summary>
    public class UpdateProjectStatusDto
    {
        /// <summary>
        /// New status for the project
        /// </summary>
        [Required]
        public ProjectStatus Status { get; set; }
        
        /// <summary>
        /// Actual end date when completing a project
        /// </summary>
        public DateTime? ActualEndDate { get; set; }
    }
}
