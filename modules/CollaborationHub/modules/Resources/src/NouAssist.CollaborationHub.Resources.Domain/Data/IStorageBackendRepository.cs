using System;
using System.Threading.Tasks;
using NouAssist.CollaborationHub.Resources.Aggregates;
using NouAssist.CollaborationHub.Resources.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CollaborationHub.Resources.Data
{
    public interface IStorageBackendRepository : IRepository<StorageBackend, Guid>
    {
        Task<StorageBackend> GetDefaultAsync();
        Task<StorageBackend> FindByNameAsync(string name);
    }
}
