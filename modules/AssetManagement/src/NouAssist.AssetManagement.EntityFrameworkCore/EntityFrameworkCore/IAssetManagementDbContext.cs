using Microsoft.EntityFrameworkCore;
using NouAssist.AssetManagement.Resources.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.AssetManagement.EntityFrameworkCore;

[ConnectionStringName(AssetManagementDbProperties.ConnectionStringName)]
public interface IAssetManagementDbContext : IEfCoreDbContext
{
    DbSet<Resource> Resources { get; }
    DbSet<Book> Books { get; }
    DbSet<InventoryItem> InventoryItems { get; }
    DbSet<StorageBackend> StorageBackends { get; }
    DbSet<BookCollection> BookCollections { get; }
}
