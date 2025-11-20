using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.EntityFrameworkCore;

[ConnectionStringName(StartupEntrepreneurshipDbProperties.ConnectionStringName)]
public class StartupEntrepreneurshipDbContext : AbpDbContext<StartupEntrepreneurshipDbContext>, IStartupEntrepreneurshipDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public StartupEntrepreneurshipDbContext(DbContextOptions<StartupEntrepreneurshipDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureStartupEntrepreneurship();
    }
}
