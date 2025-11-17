using Microsoft.EntityFrameworkCore;
using NouAssist.SharedKernel.Aggregates;
using NouAssist.SharedKernel.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.SharedKernel.EntityFrameworkCore;

[ConnectionStringName(SharedKernelDbProperties.ConnectionStringName)]
public class SharedKernelDbContext : AbpDbContext<SharedKernelDbContext>, ISharedKernelDbContext
{
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TagCategory> TagCategories { get; set; }
    public DbSet<EntityTag> EntityTags { get; set; }
    public DbSet<Context> Contexts { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<InboxItem> InboxItems { get; set; }
    public DbSet<LogActivity> LogActivities { get; set; }

    public SharedKernelDbContext(DbContextOptions<SharedKernelDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureSharedKernel();
    }
}
