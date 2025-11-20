using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Integrations.EntityFrameworkCore;

[ConnectionStringName(IntegrationsDbProperties.ConnectionStringName)]
public interface IIntegrationsDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
