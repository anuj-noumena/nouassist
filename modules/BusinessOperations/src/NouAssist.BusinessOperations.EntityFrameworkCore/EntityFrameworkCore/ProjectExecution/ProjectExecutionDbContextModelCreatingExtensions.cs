using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for configuring the Project Execution model
    /// </summary>
    public static class ProjectExecutionDbContextModelCreatingExtensions
    {
        /// <summary>
        /// Configures the Project Execution model
        /// </summary>
        public static void ConfigureProjectExecution(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            
            // Apply all IEntityTypeConfiguration classes from this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(ProjectExecutionDbContext).Assembly);
        }
    }
}
