using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.EntityFrameworkCore;

[ConnectionStringName(WorkflowOrchestrationDbProperties.ConnectionStringName)]
public interface IWorkflowOrchestrationDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
