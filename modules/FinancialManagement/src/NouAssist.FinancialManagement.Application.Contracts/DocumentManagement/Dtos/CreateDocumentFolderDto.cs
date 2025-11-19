using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.FinancialManagement.DocumentManagement.Dtos
{
    public class CreateDocumentFolderDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(2048)]
        public string Description { get; set; }

        public Guid? ParentFolderId { get; set; }
    }
}
