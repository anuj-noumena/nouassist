using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.CollaborationHub.DiscussionThreads.EntityFrameworkCore;

public static class DiscussionThreadsDbContextModelCreatingExtensions
{
    public static void ConfigureDiscussionThreads(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(DiscussionThreadsDbProperties.DbTablePrefix + "Questions", DiscussionThreadsDbProperties.DbSchema);

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
