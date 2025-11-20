using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Configurations;
using Volo.Abp;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore
{
    public static class ElicitationDbContextModelCreatingExtensions
    {
        public static void ConfigureElicitation(
            this ModelBuilder builder,
            ElicitationDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(ElicitationDbContext).Assembly);

            // Example: Any module-specific model configurations can go here
            // ObjectExtensionManager.Instance.MapEfCoreProperty<StakeholderProfile, string>(
            //     "CustomField",
            //     (entityTypeBuilder, propertyBuilder) =>
            //     {
            //         propertyBuilder.HasMaxLength(128);
            //         propertyBuilder.IsRequired(false);
            //     }
            // );
        }
    }
}
