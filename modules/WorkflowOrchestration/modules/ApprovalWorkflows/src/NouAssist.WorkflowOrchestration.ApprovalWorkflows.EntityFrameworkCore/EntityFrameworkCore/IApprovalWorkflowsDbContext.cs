using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.EntityFrameworkCore;

[ConnectionStringName(ApprovalWorkflowsDbProperties.ConnectionStringName)]
public interface IApprovalWorkflowsDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
