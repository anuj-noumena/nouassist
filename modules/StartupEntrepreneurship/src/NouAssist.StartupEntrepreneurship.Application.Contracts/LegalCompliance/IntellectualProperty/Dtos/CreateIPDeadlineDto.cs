using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos
{
    public class CreateIPDeadlineDto
    {
        [Required]
        [StringLength(LegalComplianceConsts.MaxObligationTitleLength, MinimumLength = LegalComplianceConsts.MinObligationTitleLength)]
        public string Title { get; set; }
        
        [Required]
        public DateTime Deadline { get; set; }
        
        [StringLength(LegalComplianceConsts.MaxObligationDescriptionLength)]
        public string Description { get; set; }
        
        public bool IsRecurring { get; set; }
        
        [StringLength(50)]
        public string RecurrencePattern { get; set; }
    }
