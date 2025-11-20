using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Integrations.EntityFrameworkCore;

[ConnectionStringName(IntegrationsDbProperties.ConnectionStringName)]
public class IntegrationsDbContext : AbpDbContext<IntegrationsDbContext>, IIntegrationsDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public IntegrationsDbContext(DbContextOptions<IntegrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureIntegrations();
    }
}
