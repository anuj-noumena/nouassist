using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Infrastructure.EntityFrameworkCore;

[ConnectionStringName(InfrastructureDbProperties.ConnectionStringName)]
public interface IInfrastructureDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
