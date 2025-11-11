using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.Data;
using NouAssist.AssetManagement.Resources.Enums;
using NouAssist.AssetManagement.Resources.Permissions;
using NouAssist.AssetManagement.Resources.Resources.Dtos;
using NouAssist.AssetManagement.Resources.Services;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.AssetManagement.Resources.Resources
{
    [Authorize(ResourcesPermissions.Resources.Default)]
    public class ResourceAppService : ApplicationService, IResourceAppService
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ResourceVersioningService _versioningService;
        // Note: In real implementation, inject IStorageService for actual file operations

        public ResourceAppService(
            IResourceRepository resourceRepository,
            ResourceVersioningService versioningService)
        {
            _resourceRepository = resourceRepository;
            _versioningService = versioningService;
        }

        public async Task<ResourceDto> GetAsync(Guid id)
        {
            var resource = await _resourceRepository.GetWithVersionsAsync(id);
            
            // Check authorization
            if (!resource.HasAccessPermission(CurrentUser.Id.Value, ResourcePermission.Read) && 
                resource.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Resources:AccessDenied", "You do not have permission to access this resource.");
            }

            return MapToDto(resource);
        }

        public async Task<PagedResultDto<ResourceDto>> GetListAsync(GetResourceListDto input)
        {
            var spec = CreateListSpecification(input);
            
            var resources = await _resourceRepository.GetListAsync(spec, input.MaxResultCount);
            var totalCount = await _resourceRepository.CountAsync(spec.ToExpression());

            return new PagedResultDto<ResourceDto>(
                totalCount,
                resources.Select(MapToDto).ToList()
            );
        }

        [Authorize(ResourcesPermissions.Resources.Upload)]
        public async Task<ResourceDto> UploadAsync(UploadResourceDto input)
        {
            if (input.File == null || input.File.Length == 0)
            {
                throw new BusinessException("Resources:EmptyFile", "File cannot be empty.");
            }

            // Read file content
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await input.File.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            // Calculate checksum
            var checksum = CalculateChecksum(fileBytes);

            // Generate storage key (in real app, upload to storage service)
            var storageKey = $"{CurrentUser.Id}/{Guid.NewGuid()}/{input.File.FileName}";

            // Use domain service to create resource with version
            var resource = await _versioningService.CreateResourceWithVersionAsync(
                CurrentUser.Id.Value,
                storageKey,
                input.File.ContentType,
                fileBytes.Length,
                CurrentTenant.Id,
                input.Title ?? input.File.FileName,
                CurrentUser.Id.Value,
                checksum
            );

            if (!string.IsNullOrWhiteSpace(input.Description))
            {
                resource.SetDescription(input.Description);
            }

            if (!string.IsNullOrWhiteSpace(input.Visibility))
            {
                resource.SetVisibility(input.Visibility);
            }

            await _resourceRepository.InsertAsync(resource);

            return MapToDto(resource);
        }

        [Authorize(ResourcesPermissions.Resources.Update)]
        public async Task<ResourceDto> UpdateAsync(Guid id, UpdateResourceDto input)
        {
            var resource = await _resourceRepository.GetAsync(id);

            // Authorization check
            if (resource.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Resources:NotAuthorized", "You are not authorized to update this resource.");
            }

            if (!string.IsNullOrWhiteSpace(input.Title))
            {
                resource.SetTitle(input.Title);
            }

            if (!string.IsNullOrWhiteSpace(input.Description))
            {
                resource.SetDescription(input.Description);
            }

            if (!string.IsNullOrWhiteSpace(input.Visibility))
            {
                resource.SetVisibility(input.Visibility);
            }

            await _resourceRepository.UpdateAsync(resource);

            return MapToDto(resource);
        }

        [Authorize(ResourcesPermissions.Resources.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var resource = await _resourceRepository.GetAsync(id);

            // Authorization check
            if (resource.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Resources:NotAuthorized", "You are not authorized to delete this resource.");
            }

            resource.Delete(CurrentUser.Id.Value.ToString());
            await _resourceRepository.UpdateAsync(resource);
        }

        [Authorize(ResourcesPermissions.Resources.Download)]
        public async Task<byte[]> DownloadAsync(Guid id)
        {
            var resource = await _resourceRepository.GetAsync(id);

            // Check authorization
            if (!resource.HasAccessPermission(CurrentUser.Id.Value, ResourcePermission.Read) && 
                resource.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Resources:AccessDenied", "You do not have permission to download this resource.");
            }

            // In real implementation, retrieve file from storage service
            // For now, return empty byte array
            throw new NotImplementedException("File download not implemented. Integrate with storage service.");
        }

        [Authorize(ResourcesPermissions.Resources.ManageVersions)]
        public async Task<ResourceDto> UploadNewVersionAsync(Guid id, UploadResourceDto input)
        {
            var resource = await _resourceRepository.GetWithVersionsAsync(id);

            // Authorization check
            if (resource.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Resources:NotAuthorized", "You are not authorized to upload new versions.");
            }

            if (input.File == null || input.File.Length == 0)
            {
                throw new BusinessException("Resources:EmptyFile", "File cannot be empty.");
            }

            // Read file content
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await input.File.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var checksum = CalculateChecksum(fileBytes);
            var storageKey = $"{CurrentUser.Id}/{Guid.NewGuid()}/{input.File.FileName}";

            // Use domain service to add new version
            _versioningService.AddNewVersion(
                resource,
                storageKey,
                input.File.ContentType,
                fileBytes.Length,
                CurrentUser.Id.Value,
                checksum
            );

            await _resourceRepository.UpdateAsync(resource);

            return MapToDto(resource);
        }

        [Authorize(ResourcesPermissions.Resources.ManagePermissions)]
        public async Task<ResourceDto> GrantAccessAsync(Guid id, GrantAccessDto input)
        {
            var resource = await _resourceRepository.GetWithAclAsync(id);

            // Authorization check
            if (resource.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Resources:NotAuthorized", "Only the owner can grant access.");
            }

            resource.GrantAccess(input.UserId, input.Permission, CurrentUser.Id.Value, input.ExpiresAt);

            await _resourceRepository.UpdateAsync(resource);

            return MapToDto(resource);
        }

        [Authorize(ResourcesPermissions.Resources.ManagePermissions)]
        public async Task RevokeAccessAsync(Guid id, Guid userId)
        {
            var resource = await _resourceRepository.GetWithAclAsync(id);

            // Authorization check
            if (resource.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Resources:NotAuthorized", "Only the owner can revoke access.");
            }

            resource.RevokeAccess(userId);

            await _resourceRepository.UpdateAsync(resource);
        }

        public async Task<long> GetTotalStorageUsedAsync()
        {
            return await _resourceRepository.GetTotalSizeByOwnerAsync(CurrentUser.Id.Value);
        }

        private ISpecification<Resource> CreateListSpecification(GetResourceListDto input)
        {
            // Start with current user's resources
            var spec = new Specification<Resource>(r => r.OwnerId == CurrentUser.Id.Value);

            if (input.Kind.HasValue)
            {
                spec = spec.And(new ResourcesByKindSpecification(input.Kind.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.Visibility))
            {
                spec = spec.And(new Specification<Resource>(r => r.Visibility == input.Visibility));
            }

            if (input.FromDate.HasValue)
            {
                spec = spec.And(new Specification<Resource>(r => r.CreationTime >= input.FromDate.Value));
            }

            if (input.ToDate.HasValue)
            {
                spec = spec.And(new Specification<Resource>(r => r.CreationTime <= input.ToDate.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.SearchTerm))
            {
                spec = spec.And(new Specification<Resource>(r => 
                    r.Title.Contains(input.SearchTerm) || 
                    (r.Description != null && r.Description.Contains(input.SearchTerm))));
            }

            return spec;
        }

        private ResourceDto MapToDto(Resource resource)
        {
            var dto = ObjectMapper.Map<Resource, ResourceDto>(resource);
            dto.ReadableSize = resource.GetReadableSize();
            return dto;
        }

        private string CalculateChecksum(byte[] data)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(data);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
