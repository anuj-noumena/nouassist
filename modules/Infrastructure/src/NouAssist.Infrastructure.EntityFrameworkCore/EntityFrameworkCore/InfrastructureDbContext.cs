using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Infrastructure.EntityFrameworkCore;

[ConnectionStringName(InfrastructureDbProperties.ConnectionStringName)]
public class InfrastructureDbContext : AbpDbContext<InfrastructureDbContext>, IInfrastructureDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public InfrastructureDbContext(DbContextOptions<InfrastructureDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureInfrastructure();
    }
}
