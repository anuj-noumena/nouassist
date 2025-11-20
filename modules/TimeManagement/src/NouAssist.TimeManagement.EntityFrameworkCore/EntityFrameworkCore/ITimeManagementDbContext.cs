using Microsoft.EntityFrameworkCore;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Meetings.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TimeManagement.EntityFrameworkCore;

[ConnectionStringName(TimeManagementDbProperties.ConnectionStringName)]
public interface ITimeManagementDbContext : IEfCoreDbContext
{
    DbSet<TimeManagement.Calendar.Aggregates.Calendar> Calendars { get; }
    DbSet<CalendarEvent> CalendarEvents { get; }
    DbSet<Timeblock> Timeblocks { get; }
    DbSet<TimeblockTemplate> TimeblockTemplates { get; }
    DbSet<SchedulingRequest> SchedulingRequests { get; }
    DbSet<TimeEntry> TimeEntries { get; }
    DbSet<UserAvailability> UserAvailabilities { get; }
    
    // Meetings
    DbSet<Meeting> Meetings { get; }
}
