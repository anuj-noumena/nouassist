using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Resources.Storage.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Resources.Storage
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
