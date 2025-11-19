using System.ComponentModel.DataAnnotations;
using NouAssist.FinancialManagement.DocumentManagement.Enums;

namespace NouAssist.FinancialManagement.DocumentManagement.Dtos
{
    public class CreateDocumentDto
    {
        [Required]
        public byte[] FileContent { get; set; }

        [Required]
        [StringLength(512)]
        public string FileName { get; set; }

        [Required]
        [StringLength(256)]
        public string ContentType { get; set; }

        [Required]
        public DocumentType Type { get; set; }

        [StringLength(256)]
        public string EntityType { get; set; }

        [StringLength(256)]
        public string EntityId { get; set; }

        [StringLength(2048)]
        public string Description { get; set; }

        public AccessLevel AccessLevel { get; set; } = AccessLevel.Private;
    }
}
