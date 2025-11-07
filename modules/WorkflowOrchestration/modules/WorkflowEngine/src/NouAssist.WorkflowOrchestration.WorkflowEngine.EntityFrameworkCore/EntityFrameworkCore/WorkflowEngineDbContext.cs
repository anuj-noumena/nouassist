using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.EntityFrameworkCore;

[ConnectionStringName(WorkflowEngineDbProperties.ConnectionStringName)]
public class WorkflowEngineDbContext : AbpDbContext<WorkflowEngineDbContext>, IWorkflowEngineDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public WorkflowEngineDbContext(DbContextOptions<WorkflowEngineDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureWorkflowEngine();
    }
}
