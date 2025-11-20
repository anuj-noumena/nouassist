using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos
{
    public class IPValuationDto
    {
        [Required]
        [Range(0, double.MaxValue)]
        public decimal EstimatedValue { get; set; }
        
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ValuationMethod { get; set; }
        
        [StringLength(LegalComplianceConstants.MaxNotesLength)]
        public string ValuationNotes { get; set; }
    }
