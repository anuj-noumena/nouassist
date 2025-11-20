using System;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for querying tasks with filtering, sorting, and paging
    /// </summary>
    public class GetTaskListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// General filter for name or description
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by project ID
        /// </summary>
        public Guid? ProjectId { get; set; }
        
        /// <summary>
        /// Filter by phase ID
        /// </summary>
        public Guid? PhaseId { get; set; }
        
        /// <summary>
        /// Filter by task status
        /// </summary>
        public TaskStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by task priority
        /// </summary>
        public TaskPriority? Priority { get; set; }
        
        /// <summary>
        /// Filter by assigned user ID
        /// </summary>
        public Guid? AssignedToUserId { get; set; }
        
        /// <summary>
        /// Filter by due date range (from)
        /// </summary>
        public DateTime? DueDateFrom { get; set; }
        
        /// <summary>
        /// Filter by due date range (to)
        /// </summary>
        public DateTime? DueDateTo { get; set; }
    }
}
