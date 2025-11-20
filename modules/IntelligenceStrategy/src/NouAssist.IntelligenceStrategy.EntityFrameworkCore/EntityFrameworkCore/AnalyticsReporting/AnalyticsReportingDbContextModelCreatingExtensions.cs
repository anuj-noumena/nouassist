using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore
{
    /// <summary>
    /// Model creating extensions for AnalyticsReporting DbContext
    /// </summary>
    public static class AnalyticsReportingDbContextModelCreatingExtensions
    {
        /// <summary>
        /// Configures the AnalyticsReporting module for the specified model builder
        /// </summary>
        public static void ConfigureAnalyticsReporting(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(AnalyticsReportingDbContext).Assembly);
        }
    }
}
