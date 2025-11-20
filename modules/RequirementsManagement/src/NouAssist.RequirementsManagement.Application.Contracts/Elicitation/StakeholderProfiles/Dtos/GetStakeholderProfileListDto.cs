using NouAssist.RequirementsManagement.Elicitation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Elicitation.StakeholderProfiles.Dtos
{
    /// <summary>
    /// DTO for filtering and paging stakeholder profile lists
    /// </summary>
    public class GetStakeholderProfileListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public StakeholderRole? Role { get; set; }
        public string? Organization { get; set; }
        public bool? IsActive { get; set; }
        public int? MinInfluenceLevel { get; set; }
        public int? MinInterestLevel { get; set; }
    }
}
