using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.EntityFrameworkCore;

[ConnectionStringName(ActivityTrackingDbProperties.ConnectionStringName)]
public class ActivityTrackingDbContext : AbpDbContext<ActivityTrackingDbContext>, IActivityTrackingDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public ActivityTrackingDbContext(DbContextOptions<ActivityTrackingDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureActivityTracking();
    }
}
