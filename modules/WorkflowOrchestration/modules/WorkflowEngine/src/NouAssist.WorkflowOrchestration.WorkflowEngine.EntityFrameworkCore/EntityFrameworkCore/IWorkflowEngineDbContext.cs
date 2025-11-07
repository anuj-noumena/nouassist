using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.EntityFrameworkCore;

[ConnectionStringName(WorkflowEngineDbProperties.ConnectionStringName)]
public interface IWorkflowEngineDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
