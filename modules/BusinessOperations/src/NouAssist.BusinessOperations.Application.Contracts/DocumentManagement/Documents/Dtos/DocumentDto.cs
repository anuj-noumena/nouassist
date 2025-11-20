using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.DocumentManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos
{
    /// <summary>
    /// DTO for returning document information
    /// </summary>
    public class DocumentDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Filename of the document
        /// </summary>
        public string Filename { get; set; }
        
        /// <summary>
        /// MIME type of the document
        /// </summary>
        public string MimeType { get; set; }
        
        /// <summary>
        /// Size of the document in bytes
        /// </summary>
        public long FileSize { get; set; }
        
        /// <summary>
        /// ID of the user who uploaded the document
        /// </summary>
        public Guid UploadedByUserId { get; set; }
        
        /// <summary>
        /// When the document was uploaded
        /// </summary>
        public DateTime UploadedAt { get; set; }
        
        /// <summary>
        /// Current status of the document
        /// </summary>
        public DocumentStatus Status { get; set; }
        
        /// <summary>
        /// Current version number
        /// </summary>
        public string VersionNumber { get; set; }
        
        /// <summary>
        /// Count of total versions
        /// </summary>
        public int VersionCount { get; set; }
        
        /// <summary>
        /// Access grants for the document
        /// </summary>
        public List<DocumentAccessGrantDto> AccessGrants { get; set; } = new();
    }
}
