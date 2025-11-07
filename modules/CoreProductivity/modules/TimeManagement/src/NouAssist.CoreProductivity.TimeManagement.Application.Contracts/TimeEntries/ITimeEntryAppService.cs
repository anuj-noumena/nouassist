using System;
using System.Threading.Tasks;
using NouAssist.CoreProductivity.TimeManagement.TimeEntries.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.TimeManagement.TimeEntries
{
    public interface ITimeEntryAppService : IApplicationService
    {
        Task<TimeEntryDto> GetAsync(Guid id);
        Task<TimeEntryDto> GetRunningEntryAsync();
        Task<PagedResultDto<TimeEntryDto>> GetListAsync(GetTimeEntryListDto input);
        Task<TimeEntryDto> CreateAsync(CreateTimeEntryDto input);
        Task<TimeEntryDto> UpdateAsync(Guid id, UpdateTimeEntryDto input);
        Task DeleteAsync(Guid id);
        Task<TimeEntryDto> StopAsync(Guid id);
    }
}
