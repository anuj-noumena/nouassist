using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore;

[ConnectionStringName(TimeManagementDbProperties.ConnectionStringName)]
public class TimeManagementDbContext : AbpDbContext<TimeManagementDbContext>, ITimeManagementDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

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
