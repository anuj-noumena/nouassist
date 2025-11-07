using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.EntityFrameworkCore;

[ConnectionStringName(ReflectionAndSupportDbProperties.ConnectionStringName)]
public class ReflectionAndSupportDbContext : AbpDbContext<ReflectionAndSupportDbContext>, IReflectionAndSupportDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public ReflectionAndSupportDbContext(DbContextOptions<ReflectionAndSupportDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureReflectionAndSupport();
    }
}
