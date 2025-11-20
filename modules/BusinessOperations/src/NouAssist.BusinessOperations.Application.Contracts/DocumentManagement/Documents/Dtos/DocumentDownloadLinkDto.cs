using System;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos
{
    /// <summary>
    /// DTO for a document download link
    /// </summary>
    public class DocumentDownloadLinkDto
    {
        /// <summary>
        /// Download URL for the document
        /// </summary>
        public string DownloadUrl { get; set; }
        
        /// <summary>
        /// Filename of the document
        /// </summary>
        public string Filename { get; set; }
        
        /// <summary>
        /// MIME type of the document
        /// </summary>
        public string MimeType { get; set; }
        
        /// <summary>
        /// When the link expires
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}
