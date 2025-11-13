using Microsoft.EntityFrameworkCore;
using NouAssist.Collaboration.Comments.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Collaboration.Comments.EntityFrameworkCore
{
    [ConnectionStringName(CommentsDbProperties.ConnectionStringName)]
    public interface ICommentsDbContext : IEfCoreDbContext
    {
        DbSet<Comment> Comments { get; }
    }
}
