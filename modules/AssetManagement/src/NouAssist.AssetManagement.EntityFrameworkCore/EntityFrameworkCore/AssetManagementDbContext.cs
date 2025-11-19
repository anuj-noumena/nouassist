using Microsoft.EntityFrameworkCore;
using NouAssist.AssetManagement.Resources.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.AssetManagement.EntityFrameworkCore;

[ConnectionStringName(AssetManagementDbProperties.ConnectionStringName)]
public class AssetManagementDbContext : AbpDbContext<AssetManagementDbContext>, IAssetManagementDbContext
{
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<StorageBackend> StorageBackends { get; set; }
    public DbSet<BookCollection> BookCollections { get; set; }

    public AssetManagementDbContext(DbContextOptions<AssetManagementDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureAssetManagement();
    }
}
