using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.EntityFrameworkCore;

[ConnectionStringName(CollaborationHubDbProperties.ConnectionStringName)]
public class CollaborationHubDbContext : AbpDbContext<CollaborationHubDbContext>, ICollaborationHubDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public CollaborationHubDbContext(DbContextOptions<CollaborationHubDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureCollaborationHub();
    }
}
