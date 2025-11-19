using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.FinancialManagement.DocumentManagement.Enums;

namespace NouAssist.FinancialManagement.DocumentManagement.Dtos
{
    public class UpdateDocumentDto
    {
        [StringLength(2048)]
        public string Description { get; set; }

        public AccessLevel? AccessLevel { get; set; }

        public Dictionary<string, string> Tags { get; set; }
    }
}
