using System;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorGroups.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorGroups
{
    /// <summary>
    /// Application service interface for investor group management
    /// </summary>
    public interface IInvestorGroupAppService : IApplicationService
    {
        // CRUD Operations
        Task<InvestorGroupDto> GetAsync(Guid id);
        Task<InvestorGroupDto> GetWithMembersAsync(Guid id);
        Task<PagedResultDto<InvestorGroupDto>> GetListAsync(GetInvestorGroupListDto input);
        Task<PagedResultDto<InvestorGroupDto>> GetActiveAsync(PagedAndSortedResultRequestDto input);
        Task<InvestorGroupDto> CreateAsync(CreateInvestorGroupDto input);
        Task<InvestorGroupDto> UpdateAsync(Guid id, UpdateInvestorGroupDto input);
        Task DeleteAsync(Guid id);

        // Member Management
        Task AddInvestorAsync(Guid id, AddInvestorToGroupDto input);
        Task RemoveInvestorAsync(Guid id, RemoveInvestorFromGroupDto input);
        Task<PagedResultDto<InvestorGroupMemberDto>> GetMembersAsync(Guid id, PagedAndSortedResultRequestDto input);

        // Status Management
        Task ActivateAsync(Guid id);
        Task DeactivateAsync(Guid id);

        // Validation
        Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null);

        // Bulk Operations
        Task<PagedResultDto<InvestorGroupDto>> GetGroupsForInvestorAsync(Guid investorId, PagedAndSortedResultRequestDto input);
    }
}
