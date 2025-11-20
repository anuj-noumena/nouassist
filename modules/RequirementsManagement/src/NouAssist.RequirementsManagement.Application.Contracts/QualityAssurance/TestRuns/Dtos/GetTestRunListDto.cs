using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestRuns.Dtos
{
    /// <summary>
    /// Input for filtering and paging test runs list
    /// </summary>
    public class GetTestRunListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by test cycle ID
        /// </summary>
        public Guid TestCycleId { get; set; }
        
        /// <summary>
        /// Filter by test result
        /// </summary>
        public TestRunResult? Result { get; set; }
        
        /// <summary>
        /// Filter by executor ID
        /// </summary>
        public Guid? ExecutorId { get; set; }
    }
}
