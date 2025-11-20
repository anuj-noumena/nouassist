using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for configuring CapTable DbContext model
    /// </summary>
    public static class CapTableDbContextModelCreatingExtensions
    {
        public static void ConfigureCapTable(
            this ModelBuilder builder,
            CapTableDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(CapTableDbContext).Assembly);

            // Any other module-specific model configurations can go here
        }
    }
}
