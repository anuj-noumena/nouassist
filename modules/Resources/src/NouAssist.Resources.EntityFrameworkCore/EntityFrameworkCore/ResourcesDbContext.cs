using Microsoft.EntityFrameworkCore;
using NouAssist.Resources.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Resources.EntityFrameworkCore
{
    [ConnectionStringName(ResourcesDbProperties.ConnectionStringName)]
    public class ResourcesDbContext : AbpDbContext<ResourcesDbContext>, IResourcesDbContext
    {
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<StorageBackend> StorageBackends { get; set; }
        public DbSet<BookCollection> BookCollections { get; set; }

        public ResourcesDbContext(DbContextOptions<ResourcesDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureResources(this);
        }
    }
}
