using System;
using System.Threading.Tasks;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Resources.Data
{
    public interface IStorageBackendRepository : IRepository<StorageBackend, Guid>
    {
        Task<StorageBackend> GetDefaultAsync();
        Task<StorageBackend> FindByNameAsync(string name);
    }
}
