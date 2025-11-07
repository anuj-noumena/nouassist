using System;
using System.Threading.Tasks;
using NouAssist.CoreProductivity.TimeManagement.SchedulingRequests.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.TimeManagement.SchedulingRequests
{
    public interface ISchedulingRequestAppService : IApplicationService
    {
        Task<SchedulingRequestDto> GetAsync(Guid id);
        Task<PagedResultDto<SchedulingRequestDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<SchedulingRequestDto> CreateAsync(CreateSchedulingRequestDto input);
        Task<SchedulingRequestDto> AddSlotAsync(Guid id, AddSchedulingSlotDto input);
        Task<SchedulingRequestDto> ScheduleAsync(Guid id, ScheduleRequestDto input);
        Task<SchedulingRequestDto> CancelAsync(Guid id);
    }
}
