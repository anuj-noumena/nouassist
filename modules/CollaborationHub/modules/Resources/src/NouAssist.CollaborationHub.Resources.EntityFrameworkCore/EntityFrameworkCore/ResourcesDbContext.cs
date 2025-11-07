using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.Resources.EntityFrameworkCore;

[ConnectionStringName(ResourcesDbProperties.ConnectionStringName)]
public class ResourcesDbContext : AbpDbContext<ResourcesDbContext>, IResourcesDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public ResourcesDbContext(DbContextOptions<ResourcesDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureResources();
    }
}
