using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement;
using Volo.Abp;

namespace NouAssist.WorkManagement.EntityFrameworkCore;

public static class WorkManagementDbContextModelCreatingExtensions
{
    public static void ConfigureWorkManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ApplyConfigurationsFromAssembly(typeof(WorkManagementDbContext).Assembly);

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(WorkManagementDbProperties.DbTablePrefix + "Questions", WorkManagementDbProperties.DbSchema);

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
