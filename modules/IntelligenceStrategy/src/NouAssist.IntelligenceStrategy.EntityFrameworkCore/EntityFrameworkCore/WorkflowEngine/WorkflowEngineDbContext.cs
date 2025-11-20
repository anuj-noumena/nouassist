using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore
{
    /// <summary>
    /// DbContext for the Workflow Engine module
    /// </summary>
    [ConnectionStringName(WorkflowEngineDbProperties.ConnectionStringName)]
    public class WorkflowEngineDbContext : AbpDbContext<WorkflowEngineDbContext>, IWorkflowEngineDbContext
    {
        /// <summary>
        /// Workflow definitions
        /// </summary>
        public DbSet<WorkflowDefinition> WorkflowDefinitions { get; set; } = null!;
        
        /// <summary>
        /// Workflow templates
        /// </summary>
        public DbSet<WorkflowTemplate> WorkflowTemplates { get; set; } = null!;
        
        /// <summary>
        /// Workflow instances
        /// </summary>
        public DbSet<WorkflowInstance> WorkflowInstances { get; set; } = null!;

        /// <summary>
        /// Creates a new instance of <see cref="WorkflowEngineDbContext"/>
        /// </summary>
        /// <param name="options">The options for this context</param>
        public WorkflowEngineDbContext(DbContextOptions<WorkflowEngineDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configures the model
        /// </summary>
        /// <param name="builder">The model builder</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly
            builder.ConfigureWorkflowEngine(this);
        }
    }
}
