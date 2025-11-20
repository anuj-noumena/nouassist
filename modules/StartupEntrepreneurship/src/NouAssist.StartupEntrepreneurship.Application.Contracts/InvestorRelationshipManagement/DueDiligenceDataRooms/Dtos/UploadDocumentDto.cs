using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO for uploading a document to data room
    /// </summary>
    public class UploadDocumentDto
    {
        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public DocumentType Type { get; set; }

        // File content would be handled separately via IFormFile
        // This DTO represents the metadata
    }
}
