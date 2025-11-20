using NouAssist.RequirementsManagement.Specification.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    /// <summary>
    /// DTO for filtering and sorting requirements list
    /// </summary>
    public class GetRequirementListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public RequirementType? Type { get; set; }
        public RequirementStatus? Status { get; set; }
        public RequirementPriority? Priority { get; set; }
        public ScopeStatus? ScopeStatus { get; set; }
        public bool? WithAcceptanceCriteria { get; set; }
        public bool? WithBusinessObjectives { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public Guid? CreatorId { get; set; }
    }
}
