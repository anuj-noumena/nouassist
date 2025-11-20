using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos
{
    /// <summary>
    /// DTO for getting a document download link
    /// </summary>
    public class GetDocumentDownloadLinkDto
    {
        /// <summary>
        /// ID of the document
        /// </summary>
        [Required]
        public Guid DocumentId { get; set; }
        
        /// <summary>
        /// Major version number (optional, gets latest if not specified)
        /// </summary>
        public int? VersionNumber { get; set; }
        
        /// <summary>
        /// Expiration time for the link in minutes (default: 60)
        /// </summary>
        public int ExpirationMinutes { get; set; } = 60;
    }
}
