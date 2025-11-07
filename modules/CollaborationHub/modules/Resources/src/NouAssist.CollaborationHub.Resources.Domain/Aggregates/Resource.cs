using System;
using System.Collections.Generic;
using NouAssist.CollaborationHub.Resources.Entities;
using NouAssist.CollaborationHub.Resources.Enums;
using NouAssist.CollaborationHub.Resources.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CollaborationHub.Resources.Aggregates
{
    public class Resource : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public const int MaxTitleLength = 500;

        public Guid OwnerId { get; private set; }
        public ResourceKind Kind { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string MimeType { get; private set; }
        public long? SizeBytes { get; private set; }
        public string Checksum { get; private set; }
        public Guid? CurrentVersionId { get; private set; }
        public Guid? StorageBackendId { get; private set; }
        public string Visibility { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<ResourceVersion> _versions = new();
        public IReadOnlyCollection<ResourceVersion> Versions => _versions.AsReadOnly();

        private readonly List<ResourceAcl> _accessControlList = new();
        public IReadOnlyCollection<ResourceAcl> AccessControlList => _accessControlList.AsReadOnly();

        protected Resource() { /* ORM */ }

        public Resource(
            Guid id,
            Guid ownerId,
            ResourceKind kind,
            Guid? tenantId,
            string title = null,
            string mimeType = null,
            long? sizeBytes = null) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Kind = kind;
            TenantId = tenantId;
            Title = title;
            MimeType = mimeType;
            SizeBytes = sizeBytes;
            Visibility = "private";
        }

        public void SetTitle(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                Check.Length(title, nameof(title), MaxTitleLength);
                Title = title;
            }
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetMetadata(string mimeType, long sizeBytes, string checksum = null)
        {
            MimeType = mimeType;
            SizeBytes = sizeBytes;
            
            if (!string.IsNullOrWhiteSpace(checksum))
            {
                Checksum = checksum;
            }
        }

        public ResourceVersion AddVersion(
            string storageKey,
            int versionNumber,
            string mimeType,
            long sizeBytes,
            Guid? uploadedBy = null,
            string checksum = null)
        {
            var version = new ResourceVersion(
                Id,
                storageKey,
                versionNumber,
                mimeType,
                sizeBytes,
                uploadedBy,
                checksum
            );

            _versions.Add(version);
            
            // Update resource metadata
            SetMetadata(mimeType, sizeBytes, checksum);
            
            return version;
        }

        public void SetCurrentVersion(Guid versionId)
        {
            CurrentVersionId = Check.NotNull(versionId, nameof(versionId));
        }

        public void GrantAccess(Guid userId, ResourcePermission permission, Guid? grantedBy = null, DateTime? expiresAt = null)
        {
            // Check if ACL already exists
            var existingAcl = _accessControlList.Find(acl => acl.GrantedToUserId == userId);
            if (existingAcl != null)
            {
                existingAcl.UpgradePermission(permission);
                if (expiresAt.HasValue)
                {
                    existingAcl.ExtendExpiry(expiresAt.Value);
                }
                return;
            }

            var acl = new ResourceAcl(Id, userId, permission, grantedBy, expiresAt);
            _accessControlList.Add(acl);
        }

        public void RevokeAccess(Guid userId)
        {
            var acl = _accessControlList.Find(a => a.GrantedToUserId == userId);
            if (acl != null)
            {
                _accessControlList.Remove(acl);
            }
        }

        public void SetVisibility(string visibility)
        {
            Check.NotNullOrWhiteSpace(visibility, nameof(visibility));
            Visibility = visibility;
        }

        public void SetStorageBackend(Guid storageBackendId)
        {
            StorageBackendId = storageBackendId;
        }

        public bool HasCurrentVersion => CurrentVersionId.HasValue;
        public bool IsFile => Kind == ResourceKind.File;
        public bool IsLink => Kind == ResourceKind.Link;
        public bool IsImage => Kind == ResourceKind.Image;
        public bool IsDocument => Kind == ResourceKind.Document;
        public bool IsPublic => Visibility == "public";
        public bool IsPrivate => Visibility == "private";

        public string GetReadableSize()
        {
            if (!SizeBytes.HasValue) return "Unknown";
            
            var bytes = SizeBytes.Value;
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
            if (bytes < 1024 * 1024 * 1024) return $"{bytes / (1024.0 * 1024.0):F1} MB";
            return $"{bytes / (1024.0 * 1024.0 * 1024.0):F1} GB";
        }

        public bool HasAccessPermission(Guid userId, ResourcePermission requiredPermission)
        {
            if (OwnerId == userId) return true;

            var acl = _accessControlList.Find(a => a.GrantedToUserId == userId && a.IsActive);
            if (acl == null) return false;

            return requiredPermission switch
            {
                ResourcePermission.Read => acl.CanRead,
                ResourcePermission.Write => acl.CanWrite,
                ResourcePermission.Admin => acl.CanAdmin,
                _ => false
            };
        }
    }
}
