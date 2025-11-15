using System;
using System.Collections.Generic;
using NouAssist.DocumentManagement.Domain.Shared.Events;
using NouAssist.DocumentManagement.Enums;
using NouAssist.DocumentManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.DocumentManagement.Aggregates
{
    public class Document : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public FileMetadata Metadata { get; private set; }
        public StorageLocation StorageLocation { get; private set; }
        public DocumentType Type { get; private set; }
        public DocumentStatus Status { get; private set; }
        public AccessLevel AccessLevel { get; private set; }
        public EntityReference EntityReference { get; private set; }
        public OCRResult OCRResult { get; private set; }
        public string Description { get; set; }
        public Dictionary<string, string> Tags { get; private set; }
        public DateTime? ArchivedAt { get; private set; }
        public int DownloadCount { get; private set; }
        public DateTime? LastAccessedAt { get; private set; }

        protected Document() { }

        public Document(
            Guid id,
            Guid? tenantId,
            FileMetadata metadata,
            StorageLocation storageLocation,
            DocumentType type,
            EntityReference entityReference = null,
            AccessLevel accessLevel = AccessLevel.Private,
            string description = null) : base(id)
        {
            TenantId = tenantId;
            Metadata = Check.NotNull(metadata, nameof(metadata));
            StorageLocation = Check.NotNull(storageLocation, nameof(storageLocation));
            Type = type;
            Status = DocumentStatus.Uploading;
            AccessLevel = accessLevel;
            EntityReference = entityReference;
            Description = description;
            Tags = new Dictionary<string, string>();
            DownloadCount = 0;
        }

        public void MarkAsAvailable()
        {
            if (Status != DocumentStatus.Uploading)
            {
                return;
            }

            Status = DocumentStatus.Available;

            AddDistributedEvent(new DocumentUploadedEto
            {
                DocumentId = Id,
                TenantId = TenantId,
                FileName = Metadata.FileName,
                ContentType = Metadata.ContentType,
                FileSize = Metadata.FileSize,
                DocumentType = Type,
                UploadedBy = CreatorId.Value,
                UploadedAt = CreationTime,
                EntityType = EntityReference?.EntityType,
                EntityId = EntityReference?.EntityId
            });
        }

        public void AttachOCRResult(OCRResult ocrResult)
        {
            if (Status == DocumentStatus.Deleted)
            {
                throw new BusinessException("DocumentManagement:CannotProcessDeletedDocument");
            }

            if (OCRResult != null)
            {
                throw new BusinessException("DocumentManagement:DocumentAlreadyProcessed");
            }

            OCRResult = Check.NotNull(ocrResult, nameof(ocrResult));
            Status = DocumentStatus.Available;

            AddDistributedEvent(new DocumentOCRCompletedEto
            {
                DocumentId = Id,
                TenantId = TenantId,
                ExtractedText = ocrResult.ExtractedText,
                ExtractedData = ocrResult.ExtractedData,
                ConfidenceScore = ocrResult.ConfidenceScore,
                ProcessedAt = ocrResult.ProcessedAt
            });
        }

        public void AddTag(string key, string value)
        {
            Check.NotNullOrWhiteSpace(key, nameof(key));
            Tags[key] = value ?? string.Empty;
        }

        public void RemoveTag(string key)
        {
            Tags.Remove(key);
        }

        public void Archive()
        {
            Status = DocumentStatus.Archived;
            ArchivedAt = Clock.Now;
        }

        public void Delete(Guid deletedBy, string reason = null)
        {
            Status = DocumentStatus.Deleted;

            AddDistributedEvent(new DocumentDeletedEto
            {
                DocumentId = Id,
                TenantId = TenantId,
                FileName = Metadata.FileName,
                DeletedBy = deletedBy,
                DeletedAt = Clock.Now,
                Reason = reason
            });
        }

        public void RecordAccess()
        {
            DownloadCount++;
            LastAccessedAt = Clock.Now;
        }

        public void ChangeAccessLevel(AccessLevel newAccessLevel)
        {
            AccessLevel = newAccessLevel;
        }

        public bool IsOwnedBy(Guid userId)
        {
            return CreatorId.HasValue && CreatorId.Value == userId;
        }
    }
}
