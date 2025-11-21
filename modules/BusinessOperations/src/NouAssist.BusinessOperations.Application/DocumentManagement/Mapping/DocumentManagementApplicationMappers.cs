using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos;
using NouAssist.BusinessOperations.DocumentManagement.Entities;
using NouAssist.BusinessOperations.DocumentManagement.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.BusinessOperations.DocumentManagement.Mapping
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DocumentToDocumentDtoMapper : MapperBase<Document, DocumentDto>
    {
        public override partial DocumentDto Map(Document source);

        public override partial void Map(Document source, DocumentDto destination);

        public override void AfterMap(Document source, DocumentDto destination)
        {
            destination.Filename = source.Metadata.Filename;
            destination.MimeType = source.Metadata.MimeType;
            destination.FileSize = source.Metadata.FileSize;
            destination.UploadedByUserId = source.Metadata.UploadedByUserId;
            destination.UploadedAt = source.Metadata.UploadedAt;
            destination.VersionNumber = source.CurrentVersionNumber.ToString();
            destination.VersionCount = source.Versions.Count;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AccessGrantToDocumentAccessGrantDtoMapper : MapperBase<AccessGrant, DocumentAccessGrantDto>
    {
        public override partial DocumentAccessGrantDto Map(AccessGrant source);

        public override partial void Map(AccessGrant source, DocumentAccessGrantDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DocumentVersionToDocumentVersionDtoMapper : MapperBase<DocumentVersion, DocumentVersionDto>
    {
        public override partial DocumentVersionDto Map(DocumentVersion source);

        public override partial void Map(DocumentVersion source, DocumentVersionDto destination);

        public override void AfterMap(DocumentVersion source, DocumentVersionDto destination)
        {
            destination.VersionNumber = source.VersionNumber.ToString();
            destination.Filename = source.Metadata.Filename;
            destination.MimeType = source.Metadata.MimeType;
            destination.FileSize = source.Metadata.FileSize;
            destination.CreatedByUserId = source.Metadata.UploadedByUserId;
            destination.CreatedAt = source.Metadata.UploadedAt;
        }
    }
}
