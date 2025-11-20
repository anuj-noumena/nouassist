using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for adding a member to a project
    /// </summary>
    public class CreateProjectMemberDto
    {
        /// <summary>
        /// ID of the user
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Role of the member in the project
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Role { get; set; }
        
        /// <summary>
        /// Date when the member joined the project
        /// </summary>
        [Required]
        public DateTime JoinDate { get; set; }
    }
}
