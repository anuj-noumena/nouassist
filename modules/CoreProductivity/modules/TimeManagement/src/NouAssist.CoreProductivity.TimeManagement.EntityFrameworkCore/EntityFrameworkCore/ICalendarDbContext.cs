using Microsoft.EntityFrameworkCore;
using NouAssist.CoreProductivity.TimeManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore
{
    [ConnectionStringName(CalendarDbProperties.ConnectionStringName)]
    public interface ICalendarDbContext : IEfCoreDbContext
    {
        DbSet<Aggregates.Calendar> Calendars { get; }
        DbSet<CalendarEvent> CalendarEvents { get; }
        DbSet<Timeblock> Timeblocks { get; }
        DbSet<TimeblockTemplate> TimeblockTemplates { get; }
        DbSet<SchedulingRequest> SchedulingRequests { get; }
        DbSet<TimeEntry> TimeEntries { get; }
        DbSet<UserAvailability> UserAvailabilities { get; }
    }
}
