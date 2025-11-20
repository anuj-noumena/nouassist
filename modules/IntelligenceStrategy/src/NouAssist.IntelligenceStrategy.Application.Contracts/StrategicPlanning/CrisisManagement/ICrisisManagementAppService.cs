using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement.Dtos;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement
{
    /// <summary>
    /// Application service interface for Crisis Management
    /// </summary>
    public interface ICrisisManagementAppService : IApplicationService
    {
        Task<CrisisManagementPlanDto> GetAsync(Guid id);
        Task<PagedResultDto<CrisisManagementPlanDto>> GetListAsync(PagedResultRequestDto input);
        Task<CrisisManagementPlanDto> CreateAsync(CreateCrisisManagementPlanDto input);
        Task<CrisisManagementPlanDto> UpdateAsync(Guid id, CreateCrisisManagementPlanDto input);
        Task DeleteAsync(Guid id);

        // Plan activation
        Task<CrisisManagementPlanDto> ActivateAsync(Guid id, ActivateCrisisPlanDto input);
        Task<CrisisManagementPlanDto> DeactivateAsync(Guid id);

        // Contingency management
        Task<CrisisManagementPlanDto> AddContingencyItemAsync(Guid planId, AddContingencyItemDto input);

        // Reporting
        Task<ListResultDto<CrisisManagementPlanDto>> GetPlansDueForReviewAsync();
        Task<ListResultDto<CrisisManagementPlanDto>> GetPlansForCrisisLevelAsync(CrisisLevel level);
        Task<CrisisManagementPlanDto> GetActivePlanAsync();
    }
}
