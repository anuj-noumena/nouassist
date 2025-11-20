using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools
{
    /// <summary>
    /// Application service interface for OptionPool management
    /// </summary>
    public interface IOptionPoolAppService : IApplicationService
    {
        Task<OptionPoolDto> GetAsync(Guid id);
        Task<PagedResultDto<OptionPoolDto>> GetListAsync(GetOptionPoolListDto input);
        Task<OptionPoolDto> CreateAsync(CreateOptionPoolDto input);
        Task<OptionPoolDto> UpdateAsync(Guid id, UpdateOptionPoolDto input);
        Task DeleteAsync(Guid id);
        Task<OptionPoolDto> ActivateAsync(Guid id);
        Task<OptionPoolDto> DeactivateAsync(Guid id);
        
        // Option pool operations
        Task GrantOptionsAsync(Guid id, GrantOptionsDto input);
        Task ExerciseOptionsAsync(Guid id, decimal quantity);
        Task ForfeitOptionsAsync(Guid id, decimal quantity);
        Task<OptionPoolDto> ExpandPoolAsync(Guid id, ExpandPoolDto input);
        
        // Query operations
        Task<decimal> GetAvailableOptionsAsync(Guid id);
        Task<decimal> GetOutstandingOptionsAsync(Guid id);
    }
}
