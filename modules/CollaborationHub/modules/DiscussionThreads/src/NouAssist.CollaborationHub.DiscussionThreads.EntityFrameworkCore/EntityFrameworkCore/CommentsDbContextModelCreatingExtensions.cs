using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.CollaborationHub.DiscussionThreads.EntityFrameworkCore
{
    public static class CommentsDbContextModelCreatingExtensions
    {
        public static void ConfigureComments(
            this ModelBuilder builder,
            CommentsDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(CommentsDbContext).Assembly);
        }
    }
}
