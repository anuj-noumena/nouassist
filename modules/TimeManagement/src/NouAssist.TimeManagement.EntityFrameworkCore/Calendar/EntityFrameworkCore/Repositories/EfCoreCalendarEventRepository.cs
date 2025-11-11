using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Calendar.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TimeManagement.Calendar.EntityFrameworkCore.Repositories
{
    public class EfCoreCalendarEventRepository 
        : EfCoreRepository<ICalendarDbContext, CalendarEvent, Guid>, ICalendarEventRepository
    {
        public EfCoreCalendarEventRepository(
            IDbContextProvider<ICalendarDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<CalendarEvent> GetWithExceptionsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(e => e.Exceptions)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<List<CalendarEvent>> GetEventsInRangeAsync(
            Guid calendarId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(e => e.CalendarId == calendarId &&
                           !e.IsCancelled &&
                           e.StartAt < endDate &&
                           e.EndAt > startDate)
                .OrderBy(e => e.StartAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CalendarEvent>> GetRecurringEventsAsync(
            Guid calendarId, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(e => e.Exceptions)
                .Where(e => e.CalendarId == calendarId && e.IsRecurring && !e.IsCancelled)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<CalendarEvent>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(e => e.Exceptions);
        }
    }
}
