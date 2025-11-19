using Microsoft.EntityFrameworkCore;
using NouAssist.TimeManagement.Calendar.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TimeManagement.EntityFrameworkCore;

[ConnectionStringName(TimeManagementDbProperties.ConnectionStringName)]
public class TimeManagementDbContext : AbpDbContext<TimeManagementDbContext>, ITimeManagementDbContext
{
    public DbSet<Calendar.Aggregates.Calendar> Calendars { get; set; }
    public DbSet<CalendarEvent> CalendarEvents { get; set; }
    public DbSet<Timeblock> Timeblocks { get; set; }
    public DbSet<TimeblockTemplate> TimeblockTemplates { get; set; }
    public DbSet<SchedulingRequest> SchedulingRequests { get; set; }
    public DbSet<TimeEntry> TimeEntries { get; set; }
    public DbSet<UserAvailability> UserAvailabilities { get; set; }

    public TimeManagementDbContext(DbContextOptions<TimeManagementDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureTimeManagement();
    }
}
