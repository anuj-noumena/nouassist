using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.Learning.EntityFrameworkCore
{
    public static class LearningDbContextModelCreatingExtensions
    {
        public static void ConfigureLearning(
            this ModelBuilder builder,
            LearningDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(LearningDbContext).Assembly);

            // Any additional module-specific configurations can go here
        }
    }
}
