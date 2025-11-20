using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for TaskAssignment entity
    /// </summary>
    public class TaskAssignmentDto : EntityDto
    {
        /// <summary>
        /// ID of the task
        /// </summary>
        public Guid TaskId { get; set; }
        
        /// <summary>
        /// ID of the assigned user
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Date when the task was assigned
        /// </summary>
        public DateTime AssignedDate { get; set; }
        
        /// <summary>
        /// Date when the task was unassigned, if applicable
        /// </summary>
        public DateTime? UnassignedDate { get; set; }
        
        /// <summary>
        /// Indicates if the assignment is active
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

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for TaskAssignment entity
    /// </summary>
    public class TaskAssignmentDto : EntityDto
    {
        /// <summary>
        /// ID of the task
        /// </summary>
        public Guid TaskId { get; set; }
        
        /// <summary>
        /// ID of the assigned user
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// User's name (for display purposes)
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// Date when the task was assigned
        /// </summary>
        public DateTime AssignedDate { get; set; }
        
        /// <summary>
        /// Date when the task was unassigned, if applicable
        /// </summary>
        public DateTime? UnassignedDate { get; set; }
        
        /// <summary>
        /// Indicates if the assignment is active
        /// </summary>
        public bool IsActive { get; set; }
    }
}
