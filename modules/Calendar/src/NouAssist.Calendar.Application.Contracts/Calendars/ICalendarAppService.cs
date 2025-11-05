using System;
using System.Threading.Tasks;
using NouAssist.Calendar.Calendars.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Calendar.Calendars
{
    public interface ICalendarAppService : IApplicationService
    {
        Task<CalendarDto> GetAsync(Guid id);
        Task<CalendarDto> GetDefaultAsync();
        Task<PagedResultDto<CalendarDto>> GetListAsync(GetCalendarListDto input);
        Task<CalendarDto> CreateAsync(CreateCalendarDto input);
        Task<CalendarDto> UpdateAsync(Guid id, UpdateCalendarDto input);
        Task DeleteAsync(Guid id);
        Task<CalendarDto> SetAsDefaultAsync(Guid id);
        Task<CalendarDto> ShareAsync(Guid id, ShareCalendarDto input);
        Task RevokeShareAsync(Guid id, Guid sharedWithUserId);
    }
}
