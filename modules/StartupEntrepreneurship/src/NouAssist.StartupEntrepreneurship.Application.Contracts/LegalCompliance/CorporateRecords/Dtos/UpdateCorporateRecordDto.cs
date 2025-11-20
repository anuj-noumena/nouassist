using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.CorporateRecords.Dtos
{
    public class UpdateCorporateRecordDto
    {
        [Required]
        [StringLength(LegalComplianceConsts.MaxRecordTitleLength, MinimumLength = LegalComplianceConsts.MinRecordTitleLength)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(LegalComplianceConsts.MaxRecordDescriptionLength)]
        public string Description { get; set; }
        
        [Required]
        public DateTime RecordDate { get; set; }
        
        [StringLength(50)]
        public string ReferenceNumber { get; set; }
    }
