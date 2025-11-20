using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Compliance.Dtos
{
    public class UpdateComplianceStatusDto
    {
        [Required]
        public ComplianceStatus NewStatus { get; set; }
        
        [StringLength(LegalComplianceConsts.MaxNotesLength)]
        public string Notes { get; set; }
    }
