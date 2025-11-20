using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.DocumentGeneration.Dtos
{
    public class CreateTemplateDto
    {
        [Required]
        [StringLength(LegalComplianceConsts.MaxTemplateNameLength)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxTemplateContentLength)]
        public string Content { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Format { get; set; }
        
        public Dictionary<string, string> Placeholders { get; set; } = new Dictionary<string, string>();
    }
