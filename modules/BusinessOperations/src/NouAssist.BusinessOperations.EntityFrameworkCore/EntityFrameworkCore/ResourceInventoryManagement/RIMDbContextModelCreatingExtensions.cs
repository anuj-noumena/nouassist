using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for configuring the RIM module's DbContext model
    /// </summary>
    public static class RIMDbContextModelCreatingExtensions
    {
        /// <summary>
        /// Configures the RIM module's entities
        /// </summary>
        /// <param name="builder">Model builder</param>
        /// <param name="dbContext">RIM DbContext</param>
        public static void ConfigureRIM(
            this ModelBuilder builder,
            RIMDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all entity configurations from this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(RIMDbContext).Assembly);
        }
    }
}
