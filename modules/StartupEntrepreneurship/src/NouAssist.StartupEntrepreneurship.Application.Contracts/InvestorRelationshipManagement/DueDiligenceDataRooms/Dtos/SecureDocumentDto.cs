using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO representing a secure document
    /// </summary>
    public class SecureDocumentDto : FullAuditedEntityDto<Guid>
    {
        public Guid DataRoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DocumentType Type { get; set; }
        public bool IsActive { get; set; }

        // Metadata
        public string OriginalFileName { get; set; }
        public string MimeType { get; set; }
        public long FileSize { get; set; }
        public string FileHash { get; set; }
        public DateTime UploadedAt { get; set; }
        public Guid UploadedBy { get; set; }
        public string UploadedByName { get; set; }

        // Access information
        public bool CanAccess { get; set; }
        public bool CanDownload { get; set; }
        public int AccessCount { get; set; }
        public DateTime? LastAccessDate { get; set; }

        // Download URL (if accessible)
        public string DownloadUrl { get; set; }
    }
}
