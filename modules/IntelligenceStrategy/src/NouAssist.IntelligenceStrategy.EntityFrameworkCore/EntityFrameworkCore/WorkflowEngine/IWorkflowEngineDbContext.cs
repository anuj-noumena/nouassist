using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore
{
    /// <summary>
    /// DbContext interface for the Workflow Engine module
    /// </summary>
    [ConnectionStringName(WorkflowEngineDbProperties.ConnectionStringName)]
    public interface IWorkflowEngineDbContext : IEfCoreDbContext
    {
        /// <summary>
        /// Workflow definitions
        /// </summary>
        DbSet<WorkflowDefinition> WorkflowDefinitions { get; }
        
        /// <summary>
        /// Workflow templates
        /// </summary>
        DbSet<WorkflowTemplate> WorkflowTemplates { get; }
        
        /// <summary>
        /// Workflow instances
        /// </summary>
        DbSet<WorkflowInstance> WorkflowInstances { get; }
    }
}
