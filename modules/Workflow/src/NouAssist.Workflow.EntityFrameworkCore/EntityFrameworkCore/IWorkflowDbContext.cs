using Microsoft.EntityFrameworkCore;
using NouAssist.Workflow.Aggregates;
using NouAssist.Workflow.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Workflow.EntityFrameworkCore;

[ConnectionStringName(WorkflowDbProperties.ConnectionStringName)]
public interface IWorkflowDbContext : IEfCoreDbContext
{
    DbSet<Aggregates.Workflow> Workflows { get; }
    DbSet<WorkflowScheme> WorkflowSchemes { get; }
    DbSet<WorkflowCustomField> WorkflowCustomFields { get; }
    DbSet<WorkflowStatus> WorkflowStatuses { get; }
    DbSet<WorkflowTransition> WorkflowTransitions { get; }
    DbSet<WorkflowAction> WorkflowActions { get; }
    DbSet<WorkflowSchemeMapping> WorkflowSchemeMappings { get; }
    DbSet<TransitionPermissionPolicy> TransitionPermissionPolicies { get; }
}
