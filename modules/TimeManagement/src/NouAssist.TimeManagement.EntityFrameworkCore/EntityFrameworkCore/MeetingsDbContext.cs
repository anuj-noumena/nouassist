using Microsoft.EntityFrameworkCore;
using NouAssist.TimeManagement.Meetings.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TimeManagement.Meetings.EntityFrameworkCore
{
    [ConnectionStringName(MeetingsDbProperties.ConnectionStringName)]
    public class MeetingsDbContext : AbpDbContext<MeetingsDbContext>, IMeetingsDbContext
    {
        public DbSet<Meeting> Meetings { get; set; }

        public MeetingsDbContext(DbContextOptions<MeetingsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureMeetings(this);
        }
    }
}
