using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.EntityFrameworkCore;

[ConnectionStringName(StartupEntrepreneurshipDbProperties.ConnectionStringName)]
public interface IStartupEntrepreneurshipDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
