using System;
using System.Collections.Generic;
using NouAssist.DocumentManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.DocumentManagement.Dtos
{
    public class DocumentDto : FullAuditedEntityDto<Guid>
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string FileExtension { get; set; }
        public DocumentType Type { get; set; }
        public DocumentStatus Status { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Tags { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public int DownloadCount { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public OCRResultDto OCRResult { get; set; }
    }

    public class OCRResultDto
    {
        public string ExtractedText { get; set; }
        public Dictionary<string, string> ExtractedData { get; set; }
        public decimal ConfidenceScore { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
