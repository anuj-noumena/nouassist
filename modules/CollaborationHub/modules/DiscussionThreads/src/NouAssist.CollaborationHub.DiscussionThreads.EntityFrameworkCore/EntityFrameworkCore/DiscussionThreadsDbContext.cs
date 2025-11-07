using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.DiscussionThreads.EntityFrameworkCore;

[ConnectionStringName(DiscussionThreadsDbProperties.ConnectionStringName)]
public class DiscussionThreadsDbContext : AbpDbContext<DiscussionThreadsDbContext>, IDiscussionThreadsDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public DiscussionThreadsDbContext(DbContextOptions<DiscussionThreadsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureDiscussionThreads();
    }
}
