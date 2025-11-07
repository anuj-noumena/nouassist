using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.EntityFrameworkCore;

[ConnectionStringName(CoreProductivityDbProperties.ConnectionStringName)]
public class CoreProductivityDbContext : AbpDbContext<CoreProductivityDbContext>, ICoreProductivityDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public CoreProductivityDbContext(DbContextOptions<CoreProductivityDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureCoreProductivity();
    }
}
