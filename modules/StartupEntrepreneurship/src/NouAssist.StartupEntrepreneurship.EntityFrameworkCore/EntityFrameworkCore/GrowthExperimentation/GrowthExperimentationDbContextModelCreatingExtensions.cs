using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore
{
    public static class GrowthExperimentationDbContextModelCreatingExtensions
    {
        public static void ConfigureGrowthExperimentation(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(GrowthExperimentationDbContext).Assembly);
        }
    }
}
