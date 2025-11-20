using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestCycles.Dtos
{
    /// <summary>
    /// Input for filtering and paging test cycles list
    /// </summary>
    public class GetTestCycleListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// General text filter for name or description
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by test plan ID
        /// </summary>
        public Guid? TestPlanId { get; set; }
        
        /// <summary>
        /// Filter by test cycle status
        /// </summary>
        public TestCycleStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by assignee ID
        /// </summary>
        public Guid? AssigneeId { get; set; }
    }
}
