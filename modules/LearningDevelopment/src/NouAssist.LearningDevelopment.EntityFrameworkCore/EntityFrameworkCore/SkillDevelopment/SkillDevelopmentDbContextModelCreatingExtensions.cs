using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.SkillDevelopment.EntityFrameworkCore
{
    public static class SkillDevelopmentDbContextModelCreatingExtensions
    {
        public static void ConfigureSkillDevelopment(
            this ModelBuilder builder,
            SkillDevelopmentDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes from this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(SkillDevelopmentDbContext).Assembly);
        }
    }
}
