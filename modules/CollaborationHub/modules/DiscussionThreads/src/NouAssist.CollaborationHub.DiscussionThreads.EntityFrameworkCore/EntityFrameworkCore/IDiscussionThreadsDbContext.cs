using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.DiscussionThreads.EntityFrameworkCore;

[ConnectionStringName(DiscussionThreadsDbProperties.ConnectionStringName)]
public interface IDiscussionThreadsDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
