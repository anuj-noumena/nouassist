using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.DocumentManagement.Constants;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos
{
    /// <summary>
    /// DTO for uploading a new document
    /// </summary>
    public class UploadDocumentDto
    {
        /// <summary>
        /// Filename for the document
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
        /// Description of the document
        /// </summary>
        [StringLength(1000)]
        public string? Description { get; set; }
        
        // The actual file content is handled separately via multipart form data
    }
}
