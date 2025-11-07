using Microsoft.EntityFrameworkCore;
using NouAssist.CollaborationHub.Resources.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.Resources.EntityFrameworkCore
{
    [ConnectionStringName(ResourcesDbProperties.ConnectionStringName)]
    public interface IResourcesDbContext : IEfCoreDbContext
    {
        DbSet<Resource> Resources { get; }
        DbSet<Book> Books { get; }
        DbSet<InventoryItem> InventoryItems { get; }
        DbSet<StorageBackend> StorageBackends { get; }
        DbSet<BookCollection> BookCollections { get; }
    }
}
