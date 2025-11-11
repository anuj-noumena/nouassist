using System;
using System.Collections.Generic;
using NouAssist.AssetManagement.Resources.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.AssetManagement.Resources.Entities
{
    public class ResourceVersion : Entity
    {
        public Guid ResourceId { get; private set; }
        public int VersionNumber { get; private set; }
        public string StorageKey { get; private set; }
        public Guid? StorageBackendId { get; private set; }
        public string MimeType { get; private set; }
        public long SizeBytes { get; private set; }
        public string Checksum { get; private set; }
        public Guid? UploadedBy { get; private set; }
        public DateTime UploadedAt { get; private set; }
        public bool IsCurrent { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }
        public OcrStatus OcrStatus { get; private set; }
        public string OcrText { get; private set; }
        public string PreviewStorageKey { get; private set; }

        protected ResourceVersion() { /* ORM */ }

        public ResourceVersion(
            Guid resourceId,
            string storageKey,
            int versionNumber,
            string mimeType,
            long sizeBytes,
            Guid? uploadedBy = null,
            string checksum = null)
        {
            ResourceId = resourceId;
            StorageKey = Check.NotNullOrWhiteSpace(storageKey, nameof(storageKey));
            VersionNumber = Check.Positive(versionNumber, nameof(versionNumber));
            MimeType = mimeType;
            SizeBytes = sizeBytes;
            UploadedBy = uploadedBy;
            Checksum = checksum;
            
            UploadedAt = DateTime.UtcNow;
            IsCurrent = false;
            OcrStatus = OcrStatus.NotApplicable;
            Metadata = new Dictionary<string, object>();
        }

        public void MarkAsCurrent()
        {
            IsCurrent = true;
        }

        public void MarkAsNotCurrent()
        {
            IsCurrent = false;
        }

        public void UpdateOcrStatus(OcrStatus status, string ocrText = null)
        {
            OcrStatus = status;
            
            if (!string.IsNullOrWhiteSpace(ocrText))
            {
                OcrText = ocrText;
            }
        }

        public void AddPreview(string previewKey)
        {
            PreviewStorageKey = Check.NotNullOrWhiteSpace(previewKey, nameof(previewKey));
        }

        public void SetStorageBackend(Guid storageBackendId)
        {
            StorageBackendId = storageBackendId;
        }

        public void AddMetadata(string key, object value)
        {
            Check.NotNullOrWhiteSpace(key, nameof(key));
            Metadata[key] = value;
        }

        public bool HasOcr => OcrStatus == OcrStatus.Done && !string.IsNullOrWhiteSpace(OcrText);
        public bool HasPreview => !string.IsNullOrWhiteSpace(PreviewStorageKey);
        public bool NeedsOcr => OcrStatus == OcrStatus.Pending;
        public bool OcrFailed => OcrStatus == OcrStatus.Failed;

        public override object[] GetKeys() => new object[] { ResourceId, VersionNumber };
    }
}
