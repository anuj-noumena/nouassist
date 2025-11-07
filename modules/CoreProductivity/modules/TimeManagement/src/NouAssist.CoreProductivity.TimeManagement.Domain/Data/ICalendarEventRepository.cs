using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CoreProductivity.TimeManagement.Data
{
    public interface ICalendarEventRepository : IRepository<Aggregates.CalendarEvent, Guid>
    {
        Task<Aggregates.CalendarEvent> GetWithExceptionsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Aggregates.CalendarEvent>> GetEventsInRangeAsync(
            Guid calendarId, 
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);
        Task<List<Aggregates.CalendarEvent>> GetRecurringEventsAsync(Guid calendarId, CancellationToken cancellationToken = default);
    }
}
