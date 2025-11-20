using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos
{
    /// <summary>
    /// DTO for document version information
    /// </summary>
    public class DocumentVersionDto : EntityDto
    {
        /// <summary>
        /// ID of the document
        /// </summary>
        public Guid DocumentId { get; set; }
        
        /// <summary>
        /// Version number (e.g., "1.0")
        /// </summary>
        public string VersionNumber { get; set; }
        
        /// <summary>
        /// Filename of this version
        /// </summary>
        public string Filename { get; set; }
        
        /// <summary>
        /// MIME type of this version
        /// </summary>
        public string MimeType { get; set; }
        
        /// <summary>
        /// Size of this version in bytes
        /// </summary>
        public long FileSize { get; set; }
        
        /// <summary>
        /// ID of the user who created this version
        /// </summary>
        public Guid CreatedByUserId { get; set; }
        
        /// <summary>
        /// When this version was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Comment about changes in this version
        /// </summary>
        public string ChangeComment { get; set; }
    }
}
