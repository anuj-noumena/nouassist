using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos
{
    public class UpdateLegalAgreementDto
    {
        [Required]
        [StringLength(LegalComplianceConsts.MaxAgreementTitleLength, MinimumLength = LegalComplianceConsts.MinAgreementTitleLength)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxAgreementDescriptionLength)]
        public string Description { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxPartyNameLength)]
        public string CounterpartyName { get; set; }
        
        [StringLength(50)]
        public string ReferenceNumber { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxJurisdictionLength)]
        public string Country { get; set; }
        
        [StringLength(LegalComplianceConsts.MaxJurisdictionLength)]
        public string State { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? Value { get; set; }
        
        [StringLength(3)]
        public string Currency { get; set; }
    }
