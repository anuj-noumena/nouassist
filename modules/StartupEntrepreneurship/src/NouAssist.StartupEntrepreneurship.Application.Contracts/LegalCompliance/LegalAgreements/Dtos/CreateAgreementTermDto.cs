using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos
{
    public class CreateAgreementTermDto
    {
        [Required]
        [StringLength(LegalComplianceConsts.MaxAgreementTitleLength)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxAgreementDescriptionLength)]
        public string Description { get; set; }
        
        public bool IsCritical { get; set; }
        
        public DateTime? DeadlineDate { get; set; }
    }
