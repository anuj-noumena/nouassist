using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos
{
    public class TerminateAgreementDto
    {
        [Required]
        public DateTime TerminationDate { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxNotesLength)]
        public string Reason { get; set; }
    }
