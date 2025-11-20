using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.CapTables.Dtos
{
    /// <summary>
    /// DTO for recording a valuation
    /// </summary>
    public class RecordValuationDto
    {
        [Required]
        public DateTime ValuationDate { get; set; }
        
        [Required]
        [Range(0.01, (double)decimal.MaxValue)]
        public decimal PreMoneyValuation { get; set; }
        
        [Range(0, (double)decimal.MaxValue)]
        public decimal Investment { get; set; } = 0;
        
        [StringLength(3)]
        public string Currency { get; set; } = "USD";
        
        [Required]
        [StringLength(256)]
        public string Source { get; set; }
        
        [StringLength(1000)]
        public string Notes { get; set; }
        
        public Guid? FundingRoundId { get; set; }
    }
}
