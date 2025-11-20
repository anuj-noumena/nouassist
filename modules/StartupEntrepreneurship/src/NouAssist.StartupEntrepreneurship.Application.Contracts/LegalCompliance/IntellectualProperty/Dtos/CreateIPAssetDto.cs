using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos
{
    public class CreateIPAssetDto
    {
        [Required]
        [StringLength(LegalComplianceConsts.MaxIPAssetNameLength, MinimumLength = LegalComplianceConsts.MinIPAssetNameLength)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxIPDescriptionLength)]
        public string Description { get; set; }
        
        [Required]
        public IntellectualPropertyType Type { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxIPIdentifierLength)]
        public string Identifier { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxJurisdictionLength)]
        public string IssuingAuthority { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxJurisdictionLength)]
        public string Country { get; set; }
        
        [StringLength(LegalComplianceConsts.MaxJurisdictionLength)]
        public string State { get; set; }
        
        [Required]
        public DateTime FilingDate { get; set; }
        
        [Required]
        public FilingStatusType InitialStatus { get; set; }
        
        public DateTime? RegistrationDate { get; set; }
        
        public DateTime? ExpirationDate { get; set; }
    }
