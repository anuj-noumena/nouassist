using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.DocumentGeneration.Dtos
{
    public class GenerateDocumentDto
    {
        [Required]
        public Guid TemplateId { get; set; }
        
        public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
    }
