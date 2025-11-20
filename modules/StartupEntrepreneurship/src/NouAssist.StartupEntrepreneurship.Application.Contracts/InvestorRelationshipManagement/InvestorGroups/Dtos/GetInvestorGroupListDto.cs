using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorGroups.Dtos
{
    /// <summary>
    /// DTO for filtering and paging investor group lists
    /// </summary>
    public class GetInvestorGroupListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public bool? IsActive { get; set; }
        public bool IncludeMembers { get; set; } = false;
    }
}
