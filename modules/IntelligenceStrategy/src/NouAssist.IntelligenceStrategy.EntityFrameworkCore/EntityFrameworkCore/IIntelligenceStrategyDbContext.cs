using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.EntityFrameworkCore;

[ConnectionStringName(IntelligenceStrategyDbProperties.ConnectionStringName)]
public interface IIntelligenceStrategyDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
