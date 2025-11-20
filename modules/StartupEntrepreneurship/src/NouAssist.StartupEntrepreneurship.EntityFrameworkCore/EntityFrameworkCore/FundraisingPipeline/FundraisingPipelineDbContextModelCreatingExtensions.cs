using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Configurations;
using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore
{
    public static class FundraisingPipelineDbContextModelCreatingExtensions
    {
        public static void ConfigureFundraisingPipeline(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(FundraisingPipelineDbContext).Assembly);

            // Any other module-specific model configurations can go here
        }
    }
}
