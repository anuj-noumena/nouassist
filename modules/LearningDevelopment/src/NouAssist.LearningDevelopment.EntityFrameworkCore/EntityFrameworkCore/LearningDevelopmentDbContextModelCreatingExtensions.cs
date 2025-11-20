using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.EntityFrameworkCore.Integration.Configurations;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using Volo.Abp;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore;

public static class LearningDevelopmentDbContextModelCreatingExtensions
{
    public static void ConfigureLearningDevelopment(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ApplyConfigurationsFromAssembly(typeof(LearningDevelopmentDbContext).Assembly);
           

        // Integration
        builder.ApplyConfiguration(new LearningPathConfiguration());
        builder.ApplyConfiguration(new LearningActivityConfiguration());
    }
}
