using System;
using System.Threading.Tasks;
using NouAssist.Calendar.Timeblocks.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Calendar.Timeblocks
{
    public interface ITimeblockAppService : IApplicationService
    {
        Task<TimeblockDto> GetAsync(Guid id);
        Task<PagedResultDto<TimeblockDto>> GetListAsync(GetTimeblockListDto input);
        Task<TimeblockDto> CreateAsync(CreateTimeblockDto input);
        Task<TimeblockDto> UpdateAsync(Guid id, UpdateTimeblockDto input);
        Task DeleteAsync(Guid id);
        Task<TimeblockDto> StartAsync(Guid id);
        Task<TimeblockDto> CompleteAsync(Guid id, CompleteTimeblockDto input);
        Task<TimeblockDto> CancelAsync(Guid id);
        Task<TimeblockDto> AddTaskAsync(Guid id, AddTaskToTimeblockDto input);
    }
}
