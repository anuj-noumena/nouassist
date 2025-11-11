using Microsoft.EntityFrameworkCore;
using NouAssist.Calendar.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Calendar.EntityFrameworkCore
{
    [ConnectionStringName(CalendarDbProperties.ConnectionStringName)]
    public class CalendarDbContext : AbpDbContext<CalendarDbContext>, ICalendarDbContext
    {
        public DbSet<Aggregates.Calendar> Calendars { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<Timeblock> Timeblocks { get; set; }
        public DbSet<TimeblockTemplate> TimeblockTemplates { get; set; }
        public DbSet<SchedulingRequest> SchedulingRequests { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<UserAvailability> UserAvailabilities { get; set; }

        public CalendarDbContext(DbContextOptions<CalendarDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureCalendar(this);
        }
    }
}
