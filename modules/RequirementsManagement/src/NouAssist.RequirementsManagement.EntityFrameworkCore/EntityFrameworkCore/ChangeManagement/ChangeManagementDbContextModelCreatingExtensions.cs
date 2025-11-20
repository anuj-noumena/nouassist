using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.RequirementsManagement.ChangeManagement.EntityFrameworkCore
{
    /// <summary>
    /// Extensions for configuring the model creating process of the Change Management DbContext
    /// </summary>
    public static class ChangeManagementDbContextModelCreatingExtensions
    {
        /// <summary>
        /// Configures the model for the Change Management module
        /// </summary>
        public static void ConfigureChangeManagement(
            this ModelBuilder builder,
            ChangeManagementDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(dbContext, nameof(dbContext));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(ChangeManagementDbContext).Assembly);
        }
    }
}
