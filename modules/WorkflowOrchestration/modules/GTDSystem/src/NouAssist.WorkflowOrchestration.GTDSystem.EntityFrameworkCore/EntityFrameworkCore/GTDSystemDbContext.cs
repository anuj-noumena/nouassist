using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.GTDSystem.EntityFrameworkCore;

[ConnectionStringName(GTDSystemDbProperties.ConnectionStringName)]
public class GTDSystemDbContext : AbpDbContext<GTDSystemDbContext>, IGTDSystemDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public GTDSystemDbContext(DbContextOptions<GTDSystemDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureGTDSystem();
    }
}
