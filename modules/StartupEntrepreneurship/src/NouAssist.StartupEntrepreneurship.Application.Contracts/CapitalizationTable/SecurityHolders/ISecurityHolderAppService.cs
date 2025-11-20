using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders
{
    /// <summary>
    /// Application service interface for SecurityHolder management
    /// </summary>
    public interface ISecurityHolderAppService : IApplicationService
    {
        Task<SecurityHolderDto> GetAsync(Guid id);
        Task<PagedResultDto<SecurityHolderDto>> GetListAsync(GetSecurityHolderListDto input);
        Task<SecurityHolderDto> CreateAsync(CreateSecurityHolderDto input);
        Task<SecurityHolderDto> UpdateAsync(Guid id, UpdateSecurityHolderDto input);
        Task DeleteAsync(Guid id);
        Task<SecurityHolderDto> ActivateAsync(Guid id);
        Task<SecurityHolderDto> DeactivateAsync(Guid id);
        
        // Vesting related operations
        Task<List<VestingScheduleDto>> GetVestingSchedulesAsync(Guid holderId);
        Task<VestingScheduleDto> AddVestingScheduleAsync(Guid holderId, AddVestingScheduleDto input);
        Task UpdateVestingAsync(Guid holderId, DateTime asOfDate);
        Task ExerciseOptionsAsync(Guid holderId, ExerciseOptionsDto input);
        
        // Query operations
        Task<decimal> GetTotalVestedSharesAsync(Guid holderId, DateTime asOfDate);
        Task<decimal> GetExercisableSharesAsync(Guid holderId, DateTime asOfDate);
    }
}
