using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.EntityFrameworkCore;

[ConnectionStringName(ApprovalWorkflowsDbProperties.ConnectionStringName)]
public class ApprovalWorkflowsDbContext : AbpDbContext<ApprovalWorkflowsDbContext>, IApprovalWorkflowsDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public ApprovalWorkflowsDbContext(DbContextOptions<ApprovalWorkflowsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureApprovalWorkflows();
    }
}
