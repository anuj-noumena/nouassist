using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos
{
    /// <summary>
    /// DTO for querying time logs with filtering, sorting, and paging
    /// </summary>
    public class GetTimeLogListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by project ID
        /// </summary>
        public Guid? ProjectId { get; set; }
        
        /// <summary>
        /// Filter by task ID
        /// </summary>
        public Guid? TaskId { get; set; }
        
        /// <summary>
        /// Filter by user ID
        /// </summary>
        public Guid? UserId { get; set; }
        
        /// <summary>
        /// Filter by log date range (from)
        /// </summary>
        public DateTime? LogDateFrom { get; set; }
        
        /// <summary>
        /// Filter by log date range (to)
        /// </summary>
        public DateTime? LogDateTo { get; set; }
    }
}
