using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos
{
    public class RenewAgreementDto
    {
        [Required]
        public DateTime NewEffectiveDate { get; set; }
        
        public DateTime? NewExpirationDate { get; set; }
        
        [StringLength(LegalComplianceConstants.MaxNotesLength)]
        public string Notes { get; set; }
    }
