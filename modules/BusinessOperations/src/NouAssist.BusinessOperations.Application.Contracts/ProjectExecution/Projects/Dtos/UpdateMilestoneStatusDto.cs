using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for updating a milestone status
    /// </summary>
    public class UpdateMilestoneStatusDto
    {
        /// <summary>
        /// ID of the milestone to update
        /// </summary>
        [Required]
        public Guid MilestoneId { get; set; }
        
        /// <summary>
        /// New completion status
        /// </summary>
        [Required]
        public bool IsCompleted { get; set; }
    }
}
