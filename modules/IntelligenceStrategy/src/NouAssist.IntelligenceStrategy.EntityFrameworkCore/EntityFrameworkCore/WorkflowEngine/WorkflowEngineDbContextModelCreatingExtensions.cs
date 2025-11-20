using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore
{
    /// <summary>
    /// Extensions for configuring the Workflow Engine DbContext model
    /// </summary>
    public static class WorkflowEngineDbContextModelCreatingExtensions
    {
        /// <summary>
        /// Configures the Workflow Engine model
        /// </summary>
        /// <param name="builder">The model builder</param>
        /// <param name="dbContext">The DB context</param>
        public static void ConfigureWorkflowEngine(
            this ModelBuilder builder,
            WorkflowEngineDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(dbContext, nameof(dbContext));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(WorkflowEngineDbContext).Assembly);
        }
    }
}
