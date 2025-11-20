using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos
{
    public class UpdateIPAssetDto
    {
        [Required]
        [StringLength(LegalComplianceConsts.MaxIPAssetNameLength, MinimumLength = LegalComplianceConsts.MinIPAssetNameLength)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxIPDescriptionLength)]
        public string Description { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxJurisdictionLength)]
        public string Country { get; set; }
        
        [StringLength(LegalComplianceConsts.MaxJurisdictionLength)]
        public string State { get; set; }
    }
