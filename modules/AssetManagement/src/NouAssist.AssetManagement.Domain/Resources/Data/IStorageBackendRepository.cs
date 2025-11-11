using System;
using System.Threading.Tasks;
using NouAssist.AssetManagement.Resources.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.AssetManagement.Resources.Data
{
    public interface IStorageBackendRepository : IRepository<StorageBackend, Guid>
    {
        Task<StorageBackend> GetDefaultAsync();
        Task<StorageBackend> FindByNameAsync(string name);
    }
}
