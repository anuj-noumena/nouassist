using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowInstance
{
    /// <summary>
    /// DTO for getting tasks assigned to a user/team/role
    /// </summary>
    public class GetAssignedTasksDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// The type of assignee
        /// </summary>
        [Required]
        public AssigneeType AssigneeType { get; set; }
        
        /// <summary>
        /// The ID of the assignee
        /// </summary>
        [Required]
        public Guid AssigneeId { get; set; }
        
        /// <summary>
        /// Filter by status
        /// </summary>
        public TaskStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by due date (from)
        /// </summary>
        public DateTime? DueDateFrom { get; set; }
        
        /// <summary>
        /// Filter by due date (to)
        /// </summary>
        public DateTime? DueDateTo { get; set; }
    }
}
