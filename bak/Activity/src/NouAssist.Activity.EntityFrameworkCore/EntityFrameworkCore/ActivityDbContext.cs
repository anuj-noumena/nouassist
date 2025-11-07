using Microsoft.EntityFrameworkCore;
using NouAssist.Activity.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Activity.EntityFrameworkCore
{
    [ConnectionStringName(ActivityDbProperties.ConnectionStringName)]
    public class ActivityDbContext : AbpDbContext<ActivityDbContext>, IActivityDbContext
    {
        public DbSet<ActivityFeed> ActivityFeeds { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<EntityHistory> EntityHistories { get; set; }

        public ActivityDbContext(DbContextOptions<ActivityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureActivity(this);
        }
    }
}
