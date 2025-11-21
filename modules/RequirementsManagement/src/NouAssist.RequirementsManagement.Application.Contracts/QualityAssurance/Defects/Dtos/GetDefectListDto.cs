using System;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos
{
    /// <summary>
    /// Input for filtering and paging defects list
    /// </summary>
    public class GetDefectListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// General text filter for title or description
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by test plan ID
        /// </summary>
        public Guid? TestPlanId { get; set; }
        
        /// <summary>
        /// Filter by test cycle ID
        /// </summary>
        public Guid? TestCycleId { get; set; }
        
        /// <summary>
        /// Filter by defect status
        /// </summary>
        public DefectStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by defect severity
        /// </summary>
        public DefectSeverity? Severity { get; set; }
        
        /// <summary>
        /// Filter by defect priority
        /// </summary>
        public DefectPriority? Priority { get; set; }
        
        /// <summary>
        /// Filter by assignee ID
        /// </summary>
        public Guid? AssigneeId { get; set; }
        
        /// <summary>
        /// Filter by version
        /// </summary>
        public string Version { get; set; }
    }
}
