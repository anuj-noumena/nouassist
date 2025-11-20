using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Elicitation.StakeholderProfiles.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Elicitation.StakeholderProfiles
{
    /// <summary>
    /// Application service interface for managing stakeholder profiles
    /// </summary>
    public interface IStakeholderProfileAppService : IApplicationService
    {
        Task<StakeholderProfileDto> GetAsync(Guid id);
        Task<PagedResultDto<StakeholderProfileDto>> GetListAsync(GetStakeholderProfileListDto input);
        Task<StakeholderProfileDto> CreateAsync(CreateStakeholderProfileDto input);
        Task<StakeholderProfileDto> UpdateAsync(Guid id, UpdateStakeholderProfileDto input);
        Task DeleteAsync(Guid id);
        Task<StakeholderProfileDto> ActivateAsync(Guid id);
        Task<StakeholderProfileDto> DeactivateAsync(Guid id);
        Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null);
        Task<ListResultDto<StakeholderProfileDto>> GetKeyStakeholdersAsync();
        Task<ListResultDto<StakeholderProfileDto>> GetStakeholdersByRoleAsync(StakeholderRole role);
    }
}
