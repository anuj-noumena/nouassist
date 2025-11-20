using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO for updating document metadata
    /// </summary>
    public class UpdateDocumentDto
    {
        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public DocumentType Type { get; set; }
    }
}
