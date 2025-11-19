using Microsoft.EntityFrameworkCore;
using NouAssist.Collaboration.Comments.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Collaboration.EntityFrameworkCore;

[ConnectionStringName(CollaborationDbProperties.ConnectionStringName)]
public class CollaborationDbContext : AbpDbContext<CollaborationDbContext>, ICollaborationDbContext
{
    public DbSet<Comment> Comments { get; set; }

    public CollaborationDbContext(DbContextOptions<CollaborationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureCollaboration();
    }
}
