using System;
using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos
{
    /// <summary>
    /// DTO for getting a list of branches
    /// </summary>
    public class GetBranchListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by branch name
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by branch status
        /// </summary>
        public BranchStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by source ID
        /// </summary>
        public Guid? SourceId { get; set; }
        
        /// <summary>
        /// Filter by source type
        /// </summary>
        public string SourceType { get; set; }
    }
}
