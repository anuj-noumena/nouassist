using Volo.Abp.Application.Dtos;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCases.Dtos
{
    /// <summary>
    /// Input DTO for filtering and paging test cases
    /// </summary>
    public class GetTestCaseListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public TestCaseStatus? Status { get; set; }
        public TestCasePriority? Priority { get; set; }
        public Guid? RequirementId { get; set; }
        public TestExecutionStatus? LastExecutionStatus { get; set; }
        public bool? RequiresAttention { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
    }
}
