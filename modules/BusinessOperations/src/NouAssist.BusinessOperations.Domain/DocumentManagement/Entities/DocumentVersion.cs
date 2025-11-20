using System;
using NouAssist.BusinessOperations.DocumentManagement.ValueObjects;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.DocumentManagement.Entities
{
    /// <summary>
    /// Represents a specific version of a document
    /// </summary>
    public class DocumentVersion : Entity, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid DocumentId { get; protected set; }
        public FileVersionNumber VersionNumber { get; protected set; }
        public DocumentMetadata Metadata { get; protected set; }
        public StorageReference StorageReference { get; protected set; }
        public string ChangeComment { get; protected set; }

        protected DocumentVersion() { }

        internal DocumentVersion(
            Guid documentId,
            FileVersionNumber versionNumber,
            DocumentMetadata metadata,
            StorageReference storageReference,
            string changeComment,
            Guid? tenantId)
        {
            DocumentId = documentId;
            VersionNumber = versionNumber;
            Metadata = metadata;
            StorageReference = storageReference;
            ChangeComment = changeComment;
            TenantId = tenantId;
        }

        public override object[] GetKeys()
        {
            return new object[] { DocumentId, VersionNumber };
        }
    }
}
