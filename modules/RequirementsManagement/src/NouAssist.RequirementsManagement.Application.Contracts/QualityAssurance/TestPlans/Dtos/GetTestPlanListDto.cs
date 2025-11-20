using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestPlans.Dtos
{
    /// <summary>
    /// Input for filtering and paging test plans list
    /// </summary>
    public class GetTestPlanListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// General text filter for name or description
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by release version
        /// </summary>
        public string ReleaseVersion { get; set; }
        
        /// <summary>
        /// Filter by test plan status
        /// </summary>
        public TestPlanStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by requirement ID
        /// </summary>
        public Guid? RequirementId { get; set; }
    }
}
