using System;
using NouAssist.BusinessOperations.DocumentManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos
{
    /// <summary>
    /// DTO for querying documents
    /// </summary>
    public class GetDocumentListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to search in filenames
        /// </summary>
        public string? Filter { get; set; }
        
        /// <summary>
        /// Filter by document status
        /// </summary>
        public DocumentStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by MIME type
        /// </summary>
        public string? MimeType { get; set; }
        
        /// <summary>
        /// Filter by uploader
        /// </summary>
        public Guid? UploadedByUserId { get; set; }
        
        /// <summary>
        /// Filter by upload date from
        /// </summary>
        public DateTime? UploadedFromDate { get; set; }
        
        /// <summary>
        /// Filter by upload date to
        /// </summary>
        public DateTime? UploadedToDate { get; set; }
    }
}
