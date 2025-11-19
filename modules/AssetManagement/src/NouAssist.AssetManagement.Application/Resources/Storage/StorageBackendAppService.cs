using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.AssetManagement.Permissions;
using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.Data;
using NouAssist.AssetManagement.Resources.Storage.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.AssetManagement.Resources.Storage
{
    [Authorize(AssetManagementPermissions.Resources.Storage.Default)]
    public class StorageBackendAppService : ApplicationService, IStorageBackendAppService
    {
        private readonly IStorageBackendRepository _storageBackendRepository;

        public StorageBackendAppService(IStorageBackendRepository storageBackendRepository)
        {
            _storageBackendRepository = storageBackendRepository;
        }

        public async Task<StorageBackendDto> GetAsync(Guid id)
        {
            var backend = await _storageBackendRepository.GetAsync(id);
            return ObjectMapper.Map<StorageBackend, StorageBackendDto>(backend);
        }

        public async Task<List<StorageBackendDto>> GetListAsync()
        {
            var backends = await _storageBackendRepository.GetListAsync();
            return ObjectMapper.Map<List<StorageBackend>, List<StorageBackendDto>>(backends);
        }

        public async Task<StorageBackendDto> GetDefaultAsync()
        {
            var defaultBackend = await _storageBackendRepository.GetDefaultAsync();
            
            if (defaultBackend == null)
            {
                throw new BusinessException("Storage:NoDefaultBackend", "No default storage backend is configured.");
            }

            return ObjectMapper.Map<StorageBackend, StorageBackendDto>(defaultBackend);
        }

        [Authorize(AssetManagementPermissions.Resources.Storage.Manage)]
        public async Task<StorageBackendDto> CreateAsync(CreateStorageBackendDto input)
        {
            // Check for duplicate name
            var existing = await _storageBackendRepository.FindByNameAsync(input.Name);
            if (existing != null)
            {
                throw new BusinessException("Storage:DuplicateName", "A storage backend with this name already exists.");
            }

            var backend = new StorageBackend(
                GuidGenerator.Create(),
                input.Name,
                input.BackendType,
                CurrentTenant.Id,
                input.Config,
                input.IsDefault
            );

            backend.SetFileSizeLimit(input.MaxFileSizeBytes);

            if (input.AllowedMimeTypes != null && input.AllowedMimeTypes.Count > 0)
            {
                backend.SetAllowedMimeTypes(input.AllowedMimeTypes);
            }

            if (input.IsDefault)
            {
                backend.SetAsDefault();
            }

            await _storageBackendRepository.InsertAsync(backend);

            return ObjectMapper.Map<StorageBackend, StorageBackendDto>(backend);
        }

        [Authorize(AssetManagementPermissions.Resources.Storage.Manage)]
        public async Task<StorageBackendDto> UpdateAsync(Guid id, UpdateStorageBackendDto input)
        {
            var backend = await _storageBackendRepository.GetAsync(id);

            // Check for duplicate name
            var existing = await _storageBackendRepository.FindByNameAsync(input.Name);
            if (existing != null && existing.Id != id)
            {
                throw new BusinessException("Storage:DuplicateName", "A storage backend with this name already exists.");
            }

            typeof(StorageBackend).GetProperty(nameof(StorageBackend.Name))?.SetValue(backend, input.Name);
            
            backend.UpdateConfig(input.Config);
            backend.SetFileSizeLimit(input.MaxFileSizeBytes);

            if (input.AllowedMimeTypes != null)
            {
                backend.SetAllowedMimeTypes(input.AllowedMimeTypes);
            }

            if (input.IsActive)
            {
                backend.Activate();
            }
            else
            {
                backend.Deactivate();
            }

            await _storageBackendRepository.UpdateAsync(backend);

            return ObjectMapper.Map<StorageBackend, StorageBackendDto>(backend);
        }

        [Authorize(AssetManagementPermissions.Resources.Storage.Manage)]
        public async Task DeleteAsync(Guid id)
        {
            var backend = await _storageBackendRepository.GetAsync(id);

            if (backend.IsDefault)
            {
                throw new BusinessException("Storage:CannotDeleteDefault", "Cannot delete the default storage backend.");
            }

            backend.Delete(CurrentUser.Id.Value.ToString());
            await _storageBackendRepository.UpdateAsync(backend);
        }

        [Authorize(AssetManagementPermissions.Resources.Storage.Manage)]
        public async Task<StorageBackendDto> SetAsDefaultAsync(Guid id)
        {
            var backend = await _storageBackendRepository.GetAsync(id);

            // Remove default flag from all others
            var allBackends = await _storageBackendRepository.GetListAsync();
            foreach (var b in allBackends)
            {
                if (b.IsDefault && b.Id != id)
                {
                    b.UnsetAsDefault();
                    await _storageBackendRepository.UpdateAsync(b);
                }
            }

            backend.SetAsDefault();
            await _storageBackendRepository.UpdateAsync(backend);

            return ObjectMapper.Map<StorageBackend, StorageBackendDto>(backend);
        }
    }
}
