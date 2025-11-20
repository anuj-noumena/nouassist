using Volo.Abp.Application.Dtos;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;

namespace NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles.Dtos
{
    /// <summary>
    /// Input DTO for filtering and paging review cycles
    /// </summary>
    public class GetReviewCycleListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public Guid? RequirementId { get; set; }
        public ReviewCycleStatus? Status { get; set; }
        public Guid? ReviewerId { get; set; }
        public bool? IsOverdue { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
    }
}
