using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.DocumentManagement.Constants;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos
{
    /// <summary>
    /// DTO for creating a new document version
    /// </summary>
    public class CreateDocumentVersionDto
    {
        /// <summary>
        /// ID of the document to version
        /// </summary>
        [Required]
        public Guid DocumentId { get; set; }
        
        /// <summary>
        /// Filename for the version
        /// </summary>
        [Required]
        [StringLength(DocumentManagementConsts.Document.MaxFilenameLength)]
        public string Filename { get; set; }
        
        /// <summary>
        /// MIME type of the document
        /// </summary>
        [Required]
        [StringLength(DocumentManagementConsts.Document.MaxMimeTypeLength)]
        public string MimeType { get; set; }
        
        /// <summary>
        /// Comment describing the changes in this version
        /// </summary>
        [Required]
        [StringLength(500)]
        public string ChangeComment { get; set; }
        
        // The actual file content is handled separately via multipart form data
    }
}
