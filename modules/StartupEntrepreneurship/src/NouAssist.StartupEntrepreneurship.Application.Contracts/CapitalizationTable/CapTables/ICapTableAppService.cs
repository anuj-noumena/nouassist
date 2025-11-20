using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.CapTables.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.CapTables
{
    /// <summary>
    /// Application service interface for CapTable management
    /// </summary>
    public interface ICapTableAppService : IApplicationService
    {
        Task<CapTableDto> GetAsync(Guid id);
        Task<PagedResultDto<CapTableDto>> GetListAsync(GetCapTableListDto input);
        Task<CapTableDto> CreateAsync(CreateCapTableDto input);
        Task<CapTableDto> UpdateAsync(Guid id, UpdateCapTableDto input);
        Task DeleteAsync(Guid id);
        Task<CapTableDto> ActivateAsync(Guid id);
        Task<CapTableDto> DeactivateAsync(Guid id);
        Task<CapTableDto> RecordValuationAsync(Guid id, RecordValuationDto input);
        Task<CapTableDto> UpdateAsOfDateAsync(Guid id, DateTime newAsOfDate);
    }
}
