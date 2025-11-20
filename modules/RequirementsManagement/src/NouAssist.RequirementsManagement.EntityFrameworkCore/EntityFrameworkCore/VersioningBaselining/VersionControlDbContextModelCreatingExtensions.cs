using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore
{
    /// <summary>
    /// Extensions for configuring the VersionControl module in the DbContext
    /// </summary>
    public static class VersionControlDbContextModelCreatingExtensions
    {
        /// <summary>
        /// Configures the VersionControl module in the DbContext
        /// </summary>
        public static void ConfigureVersionControl(
            this ModelBuilder builder,
            Action<VersionControlModelBuilderConfigurationOptions>? optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new VersionControlModelBuilderConfigurationOptions(
                VersionControlDbProperties.DbTablePrefix,
                VersionControlDbProperties.DbSchema);

            optionsAction?.Invoke(options);

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(VersionControlDbContext).Assembly);
        }
    }

    /// <summary>
    /// Configuration options for the VersionControl module
    /// </summary>
    public class VersionControlModelBuilderConfigurationOptions
    {
        public string TablePrefix { get; set; }
        public string? Schema { get; set; }

        public VersionControlModelBuilderConfigurationOptions(
            string tablePrefix = "",
            string? schema = null)
        {
            TablePrefix = tablePrefix;
            Schema = schema;
        }
    }
}
