using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.TimeManagement.Calendar.CalendarEvents.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.TimeManagement.Calendar.CalendarEvents
{
    public interface ICalendarEventAppService : IApplicationService
    {
        Task<CalendarEventDto> GetAsync(Guid id);
        Task<PagedResultDto<CalendarEventDto>> GetListAsync(GetCalendarEventListDto input);
        Task<CalendarEventDto> CreateAsync(CreateCalendarEventDto input);
        Task<CalendarEventDto> UpdateAsync(Guid id, UpdateCalendarEventDto input);
        Task DeleteAsync(Guid id);
        Task<CalendarEventDto> RescheduleAsync(Guid id, RescheduleEventDto input);
        Task<CalendarEventDto> CancelAsync(Guid id, CancelEventDto input);
        Task<CalendarEventDto> LinkEntityAsync(Guid id, LinkEntityToEventDto input);
        Task<CalendarEventDto> UnlinkEntityAsync(Guid id, Guid entityId, string entityType);
        Task<List<CalendarEventDto>> GetEventsInRangeAsync(Guid calendarId, DateTime startDate, DateTime endDate);
    }
}
