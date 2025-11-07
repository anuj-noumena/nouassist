using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.GTDSystem.EntityFrameworkCore;

[ConnectionStringName(GTDSystemDbProperties.ConnectionStringName)]
public interface IGTDSystemDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
