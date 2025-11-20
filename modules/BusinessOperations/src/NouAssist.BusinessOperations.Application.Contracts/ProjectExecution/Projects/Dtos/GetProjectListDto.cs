using System;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for querying projects with filtering, sorting, and paging
    /// </summary>
    public class GetProjectListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// General filter for name or description
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by project status
        /// </summary>
        public ProjectStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by organization ID
        /// </summary>
        public Guid? OrganizationId { get; set; }
        
        /// <summary>
        /// Filter by contact ID
        /// </summary>
        public Guid? ContactId { get; set; }
        
        /// <summary>
        /// Filter by start date range (from)
        /// </summary>
        public DateTime? StartDateFrom { get; set; }
        
        /// <summary>
        /// Filter by start date range (to)
        /// </summary>
        public DateTime? StartDateTo { get; set; }
    }
}
