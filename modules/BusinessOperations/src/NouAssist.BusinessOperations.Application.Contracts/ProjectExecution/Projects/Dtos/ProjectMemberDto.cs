using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for ProjectMember entity
    /// </summary>
    public class ProjectMemberDto : EntityDto
    {
        /// <summary>
        /// ID of the project this member belongs to
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// ID of the user
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Role of the member in the project
        /// </summary>
        public string Role { get; set; }
        
        /// <summary>
        /// Date when the member joined the project
        /// </summary>
        public DateTime JoinDate { get; set; }
        
        /// <summary>
        /// Date when the member left the project, if applicable
        /// </summary>
        public DateTime? LeaveDate { get; set; }
        
        /// <summary>
        /// Indicates if the member is active in the project
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Optional user name for display
        /// </summary>
        public string UserName { get; set; }
    }
}
using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for ProjectMember entity
    /// </summary>
    public class ProjectMemberDto : EntityDto
    {
        /// <summary>
        /// ID of the project this member belongs to
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// ID of the user
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Role of the member in the project
        /// </summary>
        public string Role { get; set; }
        
        /// <summary>
        /// Date when the member joined the project
        /// </summary>
        public DateTime JoinDate { get; set; }
        
        /// <summary>
        /// Date when the member left the project, if applicable
        /// </summary>
        public DateTime? LeaveDate { get; set; }
        
        /// <summary>
        /// Indicates if the member is active in the project
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// User's name (for display purposes)
        /// </summary>
        public string UserName { get; set; }
    }
}
