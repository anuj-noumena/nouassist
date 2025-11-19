using System;
using System.Collections.Generic;
using NouAssist.AssetManagement.Resources.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.AssetManagement.Resources.Aggregates
{
    public class StorageBackend : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public const int MaxNameLength = 100;

        public string Name { get; private set; }
        public StorageProvider BackendType { get; private set; }
        public Dictionary<string, object> Config { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDefault { get; private set; }
        public int? MaxFileSizeMB { get; private set; }
        public List<string> AllowedMimeTypes { get; private set; }
        public Guid? TenantId { get; private set; }

        protected StorageBackend() { /* ORM */ }

        public StorageBackend(
            Guid id,
            string name,
            StorageProvider backendType,
            Guid? tenantId,
            Dictionary<string, object> config = null,
            bool isDefault = false) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), MaxNameLength);
            BackendType = backendType;
            TenantId = tenantId;
            Config = config ?? new Dictionary<string, object>();
            IsDefault = isDefault;
            IsActive = true;
            AllowedMimeTypes = new List<string>();
        }

        public void MakeDefault()
        {
            IsDefault = true;
        }

        public void RemoveDefault()
        {
            IsDefault = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void SetMaxFileSize(int maxSizeMB)
        {
            MaxFileSizeMB = Check.Positive(maxSizeMB, nameof(maxSizeMB));
        }

        public void SetAllowedMimeTypes(List<string> mimeTypes)
        {
            AllowedMimeTypes = mimeTypes ?? new List<string>();
        }

        public void UpdateConfig(Dictionary<string, object> config)
        {
            Config = config ?? new Dictionary<string, object>();
        }

        public bool HasFileSizeLimit => MaxFileSizeMB.HasValue;
        public bool HasAllowedMimeTypes => AllowedMimeTypes != null && AllowedMimeTypes.Count > 0;

        public bool IsFileSizeAllowed(long sizeBytes)
        {
            if (!HasFileSizeLimit)
            {
                return true;
            }

            var sizeMB = sizeBytes / (1024.0 * 1024.0);
            return sizeMB <= MaxFileSizeMB.Value;
        }

        public bool IsMimeTypeAllowed(string mimeType)
        {
            if (!HasAllowedMimeTypes)
            {
                return true;
            }

            return AllowedMimeTypes.Contains(mimeType);
        }

        public void SetFileSizeLimit(long? maxFileSizeBytes)
        {
            if (maxFileSizeBytes.HasValue && maxFileSizeBytes.Value > 0)
            {
                MaxFileSizeMB = (int)(maxFileSizeBytes.Value / (1024 * 1024));
            }
            else
            {
                MaxFileSizeMB = null;
            }
        }

        public void SetAsDefault()
        {
            IsDefault = true;
        }

        public void UnsetAsDefault()
        {
            IsDefault = false;
        }

        public void Delete(string deletedBy)
        {
            IsDeleted = true;
            DeleterId = Guid.TryParse(deletedBy, out var id) ? id : null;
            DeletionTime = DateTime.UtcNow;
        }
    }
}
