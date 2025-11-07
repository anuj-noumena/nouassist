using Microsoft.EntityFrameworkCore;
using NouAssist.CollaborationHub.DiscussionThreads.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.DiscussionThreads.EntityFrameworkCore
{
    [ConnectionStringName(CommentsDbProperties.ConnectionStringName)]
    public interface ICommentsDbContext : IEfCoreDbContext
    {
        DbSet<Comment> Comments { get; }
    }
}
