using System;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Data;
using NouAssist.Resources.Enums;
using NouAssist.Resources.Events;
using Volo.Abp.Domain.Services;

namespace NouAssist.Resources.Services
{
    public class ResourceVersioningService : DomainService
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceVersioningService(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<Resource> CreateResourceWithVersionAsync(
            Guid ownerId,
            string storageKey,
            string mimeType,
            long sizeBytes,
            Guid? tenantId,
            string title = null,
            Guid? uploadedBy = null,
            string checksum = null)
        {
            var resource = new Resource(
                GuidGenerator.Create(),
                ownerId,
                DetermineResourceKind(mimeType),
                tenantId,
                title,
                mimeType,
                sizeBytes
            );

            var version = resource.AddVersion(
                storageKey,
                1,
                mimeType,
                sizeBytes,
                uploadedBy,
                checksum
            );

            version.MarkAsCurrent();
            resource.SetCurrentVersion(version.ResourceId);

            // Publish event
            resource.AddDistributedEvent(new ResourceUploadedEto
            {
                ResourceId = resource.Id,
                OwnerId = resource.OwnerId,
                ResourceKind = resource.Kind.ToString(),
                MimeType = resource.MimeType,
                SizeBytes = resource.SizeBytes.Value,
                TenantId = resource.TenantId,
                UploadedAt = Clock.Now
            });

            return resource;
        }

        public async Task<int> GetNextVersionNumberAsync(Resource resource)
        {
            var maxVersion = resource.Versions.Any() 
                ? resource.Versions.Max(v => v.VersionNumber) 
                : 0;
            
            return maxVersion + 1;
        }

        public void AddNewVersion(
            Resource resource,
            string storageKey,
            string mimeType,
            long sizeBytes,
            Guid? uploadedBy = null,
            string checksum = null)
        {
            var nextVersion = resource.Versions.Any() 
                ? resource.Versions.Max(v => v.VersionNumber) + 1 
                : 1;

            // Mark previous current version as not current
            var previousCurrent = resource.Versions.FirstOrDefault(v => v.IsCurrent);
            previousCurrent?.MarkAsNotCurrent();

            var newVersion = resource.AddVersion(
                storageKey,
                nextVersion,
                mimeType,
                sizeBytes,
                uploadedBy,
                checksum
            );

            newVersion.MarkAsCurrent();
            resource.SetCurrentVersion(newVersion.ResourceId);
            resource.SetMetadata(mimeType, sizeBytes, checksum);
        }

        private ResourceKind DetermineResourceKind(string mimeType)
        {
            if (string.IsNullOrWhiteSpace(mimeType))
                return ResourceKind.File;

            if (mimeType.StartsWith("image/"))
                return ResourceKind.Image;

            if (mimeType.StartsWith("video/"))
                return ResourceKind.Video;

            if (mimeType.StartsWith("audio/"))
                return ResourceKind.Audio;

            if (mimeType.Contains("pdf") || mimeType.Contains("document") || mimeType.Contains("word"))
                return ResourceKind.Document;

            if (mimeType.Contains("spreadsheet") || mimeType.Contains("excel"))
                return ResourceKind.Spreadsheet;

            if (mimeType.Contains("json"))
                return ResourceKind.Json;

            if (mimeType.Contains("markdown") || mimeType.EndsWith("md"))
                return ResourceKind.Markdown;

            if (mimeType.Contains("zip") || mimeType.Contains("tar") || mimeType.Contains("gz"))
                return ResourceKind.Archive;

            return ResourceKind.File;
        }
    }
}
