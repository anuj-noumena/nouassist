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

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(LearningDevelopmentDbProperties.DbTablePrefix + "Questions", LearningDevelopmentDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        // Integration
        builder.ApplyConfiguration(new LearningPathConfiguration());
        builder.ApplyConfiguration(new LearningActivityConfiguration());
    }
}
