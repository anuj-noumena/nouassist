using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.ReflectionAndSupport.CustomFields.EntityFrameworkCore;

public static class CustomFieldsDbContextModelCreatingExtensions
{
    public static void ConfigureCustomFields(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(CustomFieldsDbProperties.DbTablePrefix + "Questions", CustomFieldsDbProperties.DbSchema);

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
