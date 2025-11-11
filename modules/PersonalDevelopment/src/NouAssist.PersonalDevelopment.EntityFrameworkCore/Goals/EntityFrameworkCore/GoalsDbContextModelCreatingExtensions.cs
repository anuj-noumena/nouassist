using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.PersonalDevelopment.Goals.EntityFrameworkCore;

public static class GoalsDbContextModelCreatingExtensions
{
    public static void ConfigureGoals(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(GoalsDbProperties.DbTablePrefix + "Questions", GoalsDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */
    }
}
