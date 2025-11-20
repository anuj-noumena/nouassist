using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Traceability.EntityFrameworkCore
{
    /// <summary>
    /// Extensions methods for configuring the TraceabilityManagement module model
    /// </summary>
    public static class TraceabilityManagementDbContextModelCreatingExtensions
    {
        /// <summary>
        /// Configures the TraceabilityManagement module entities
        /// </summary>
        public static void ConfigureTraceabilityManagement(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all entity configurations from assembly
            builder.ApplyConfigurationsFromAssembly(typeof(TraceabilityManagementDbContext).Assembly);
        }
    }
}
