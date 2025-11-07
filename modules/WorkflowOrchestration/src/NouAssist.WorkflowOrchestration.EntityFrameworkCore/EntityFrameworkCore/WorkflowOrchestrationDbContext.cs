using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.EntityFrameworkCore;

[ConnectionStringName(WorkflowOrchestrationDbProperties.ConnectionStringName)]
public class WorkflowOrchestrationDbContext : AbpDbContext<WorkflowOrchestrationDbContext>, IWorkflowOrchestrationDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public WorkflowOrchestrationDbContext(DbContextOptions<WorkflowOrchestrationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureWorkflowOrchestration();
    }
}
