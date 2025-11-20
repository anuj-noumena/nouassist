using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos
{
    public class CreateAgreementReminderDto
    {
        [Required]
        [StringLength(LegalComplianceConsts.MaxAgreementTitleLength)]
        public string Title { get; set; }
        
        [Required]
        public DateTime ReminderDate { get; set; }
        
        [StringLength(LegalComplianceConsts.MaxAgreementDescriptionLength)]
        public string Description { get; set; }
        
        [Range(0, 365)]
        public int? DaysBeforeEvent { get; set; }
        
        public bool IsRecurring { get; set; }
        
        [StringLength(50)]
        public string RecurrencePattern { get; set; }
    }
