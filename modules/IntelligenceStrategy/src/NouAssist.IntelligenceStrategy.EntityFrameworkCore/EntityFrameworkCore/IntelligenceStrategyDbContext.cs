using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.EntityFrameworkCore;

[ConnectionStringName(IntelligenceStrategyDbProperties.ConnectionStringName)]
public class IntelligenceStrategyDbContext : AbpDbContext<IntelligenceStrategyDbContext>, IIntelligenceStrategyDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public IntelligenceStrategyDbContext(DbContextOptions<IntelligenceStrategyDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureIntelligenceStrategy();
    }
}
