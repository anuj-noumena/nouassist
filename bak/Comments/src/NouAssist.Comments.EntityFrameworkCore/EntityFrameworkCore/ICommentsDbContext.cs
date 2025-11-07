using Microsoft.EntityFrameworkCore;
using NouAssist.Comments.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Comments.EntityFrameworkCore
{
    [ConnectionStringName(CommentsDbProperties.ConnectionStringName)]
    public interface ICommentsDbContext : IEfCoreDbContext
    {
        DbSet<Comment> Comments { get; }
    }
}
