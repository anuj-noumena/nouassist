using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Compliance.Dtos
{
    public class CreateComplianceObligationDto
    {
        [Required]
        [StringLength(LegalComplianceConsts.MaxObligationTitleLength, MinimumLength = LegalComplianceConsts.MinObligationTitleLength)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxObligationDescriptionLength)]
        public string Overview { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxObligationDescriptionLength)]
        public string Requirements { get; set; }
        
        [StringLength(LegalComplianceConsts.MaxNotesLength)]
        public string Notes { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxJurisdictionLength)]
        public string Country { get; set; }
        
        [StringLength(LegalComplianceConsts.MaxJurisdictionLength)]
        public string State { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxPartyNameLength)]
        public string AuthorityName { get; set; }
        
        public DateTime? Deadline { get; set; }
        
        [StringLength(50)]
        public string ReferenceNumber { get; set; }
        
        public bool IsRecurring { get; set; }
        
        [StringLength(50)]
        public string RecurrencePattern { get; set; }
    }
