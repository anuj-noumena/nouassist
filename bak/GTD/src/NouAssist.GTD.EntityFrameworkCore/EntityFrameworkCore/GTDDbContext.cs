using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.GTD.EntityFrameworkCore;

[ConnectionStringName(GTDDbProperties.ConnectionStringName)]
public class GTDDbContext : AbpDbContext<GTDDbContext>, IGTDDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public GTDDbContext(DbContextOptions<GTDDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureGTD();
    }
}
