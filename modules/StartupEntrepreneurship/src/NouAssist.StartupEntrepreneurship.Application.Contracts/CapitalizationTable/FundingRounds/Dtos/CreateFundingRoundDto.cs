using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.FundingRounds.Dtos
{
    /// <summary>
    /// DTO for creating a new FundingRound
    /// </summary>
    public class CreateFundingRoundDto
    {
        [Required]
        public Guid CapTableId { get; set; }
        
        [Required]
        [StringLength(CapTableConsts.MaxFundingRoundNameLength)]
        public string Name { get; set; }
        
        [StringLength(CapTableConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        [Required]
        [Range(0.01, (double)decimal.MaxValue)]
        public decimal PreMoneyValuation { get; set; }
        
        [Required]
        [Range(0.01, (double)decimal.MaxValue)]
        public decimal TargetAmount { get; set; }
        
        [StringLength(3)]
        public string Currency { get; set; } = "USD";
        
        public DateTime? StartDate { get; set; }
        
        [StringLength(CapTableConsts.MaxHolderNameLength)]
        public string LeadInvestor { get; set; }
    }
}
