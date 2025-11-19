using Microsoft.EntityFrameworkCore;
using NouAssist.Collaboration.Comments.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Collaboration.EntityFrameworkCore;

[ConnectionStringName(CollaborationDbProperties.ConnectionStringName)]
public interface ICollaborationDbContext : IEfCoreDbContext
{
    DbSet<Comment> Comments { get; }
}
