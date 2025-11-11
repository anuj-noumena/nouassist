using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.Workflow.EntityFrameworkCore;

[ConnectionStringName(WorkflowDbProperties.ConnectionStringName)]
public class WorkflowDbContext : AbpDbContext<WorkflowDbContext>, IWorkflowDbContext
{
    public DbSet<Aggregates.Workflow> Workflows { get; set; }
    public DbSet<WorkflowScheme> WorkflowSchemes { get; set; }
    public DbSet<WorkflowCustomField> WorkflowCustomFields { get; set; }
    public DbSet<WorkflowStatus> WorkflowStatuses { get; set; }
    public DbSet<WorkflowTransition> WorkflowTransitions { get; set; }
    public DbSet<WorkflowAction> WorkflowActions { get; set; }
    public DbSet<WorkflowSchemeMapping> WorkflowSchemeMappings { get; set; }
    public DbSet<TransitionPermissionPolicy> TransitionPermissionPolicies { get; set; }
    
    public WorkflowDbContext(DbContextOptions<WorkflowDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureWorkflow();
    }
}
