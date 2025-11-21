using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using NouAssist.FinancialManagement.DocumentManagement.Dtos;
using NouAssist.FinancialManagement.DocumentManagement.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.FinancialManagement.DocumentManagement
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DocumentToDocumentDtoMapper : MapperBase<Document, DocumentDto>
    {
        [MapperIgnoreTarget(nameof(DocumentDto.FileName))]
        [MapperIgnoreTarget(nameof(DocumentDto.ContentType))]
        [MapperIgnoreTarget(nameof(DocumentDto.FileSize))]
        [MapperIgnoreTarget(nameof(DocumentDto.FileExtension))]
        [MapperIgnoreTarget(nameof(DocumentDto.EntityType))]
        [MapperIgnoreTarget(nameof(DocumentDto.EntityId))]
        [MapperIgnoreTarget(nameof(DocumentDto.OCRResult))]
        [MapperIgnoreTarget(nameof(DocumentDto.IsArchived))]
        public override partial DocumentDto Map(Document source);

        
        [MapperIgnoreTarget(nameof(DocumentDto.FileName))]
        [MapperIgnoreTarget(nameof(DocumentDto.ContentType))]
        [MapperIgnoreTarget(nameof(DocumentDto.FileSize))]
        [MapperIgnoreTarget(nameof(DocumentDto.FileExtension))]
        [MapperIgnoreTarget(nameof(DocumentDto.EntityType))]
        [MapperIgnoreTarget(nameof(DocumentDto.EntityId))]
        [MapperIgnoreTarget(nameof(DocumentDto.OCRResult))]
        [MapperIgnoreTarget(nameof(DocumentDto.IsArchived))]
        public override partial void Map(Document source, DocumentDto destination);

        public override void AfterMap(Document source, DocumentDto destination)
        {
            destination.FileName = source.Metadata.FileName;
            destination.ContentType = source.Metadata.ContentType;
            destination.FileSize = source.Metadata.FileSize;
            destination.FileExtension = source.Metadata.FileExtension;
            destination.EntityType = source.EntityReference != null ? source.EntityReference.EntityType : null;
            destination.EntityId = source.EntityReference != null ? source.EntityReference.EntityId : null;
            destination.OCRResult = new OCRResultDto
            {
                ExtractedText =  source.OCRResult.ExtractedText,
                ExtractedData = source.OCRResult.ExtractedData ,
                ConfidenceScore =  source.OCRResult.ConfidenceScore ,
                ProcessedAt =  source.OCRResult.ProcessedAt
            };
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class OCRResultToOCRResultDtoMapper : MapperBase<OCRResult, OCRResultDto>
    {
        public override partial OCRResultDto Map(OCRResult source);

        public override partial void Map(OCRResult source, OCRResultDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DocumentFolderToDocumentFolderDtoMapper : MapperBase<DocumentFolder, DocumentFolderDto>
    {
        [MapperIgnoreTarget(nameof(DocumentFolderDto.DocumentCount))]
        public override partial DocumentFolderDto Map(DocumentFolder source);

        [MapperIgnoreTarget(nameof(DocumentFolderDto.DocumentCount))]
        public override partial void Map(DocumentFolder source, DocumentFolderDto destination);

        public override void AfterMap(DocumentFolder source, DocumentFolderDto destination)
        {
            destination.DocumentCount = source.DocumentIds.Count;
        }
    }
}
