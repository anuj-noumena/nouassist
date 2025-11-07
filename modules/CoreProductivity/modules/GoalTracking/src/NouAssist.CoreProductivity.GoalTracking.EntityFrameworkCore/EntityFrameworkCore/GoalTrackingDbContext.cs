using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.GoalTracking.EntityFrameworkCore;

[ConnectionStringName(GoalTrackingDbProperties.ConnectionStringName)]
public class GoalTrackingDbContext : AbpDbContext<GoalTrackingDbContext>, IGoalTrackingDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public GoalTrackingDbContext(DbContextOptions<GoalTrackingDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureGoalTracking();
    }
}
