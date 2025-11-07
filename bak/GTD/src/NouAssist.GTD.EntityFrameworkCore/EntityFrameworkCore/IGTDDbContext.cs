using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.GTD.EntityFrameworkCore;

[ConnectionStringName(GTDDbProperties.ConnectionStringName)]
public interface IGTDDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
