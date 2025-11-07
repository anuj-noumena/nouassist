using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.CollaborationHub.Resources.Storage.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.Resources.Storage
{
    public interface IStorageBackendAppService : IApplicationService
    {
        Task<StorageBackendDto> GetAsync(Guid id);
        Task<List<StorageBackendDto>> GetListAsync();
        Task<StorageBackendDto> GetDefaultAsync();
        Task<StorageBackendDto> CreateAsync(CreateStorageBackendDto input);
        Task<StorageBackendDto> UpdateAsync(Guid id, UpdateStorageBackendDto input);
        Task DeleteAsync(Guid id);
        Task<StorageBackendDto> SetAsDefaultAsync(Guid id);
    }
}
