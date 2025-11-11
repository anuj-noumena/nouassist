using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.Workflow.EntityFrameworkCore;

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
