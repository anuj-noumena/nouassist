using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.FundingRounds.Dtos
{
    /// <summary>
    /// DTO for updating an existing FundingRound
    /// </summary>
    public class UpdateFundingRoundDto
    {
        [Required]
        [StringLength(CapTableConsts.MaxFundingRoundNameLength)]
        public string Name { get; set; }
        
        [StringLength(CapTableConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        [Range(0.01, (double)decimal.MaxValue)]
        public decimal? TargetAmount { get; set; }
        
        [StringLength(CapTableConsts.MaxHolderNameLength)]
        public string LeadInvestor { get; set; }
    }
}
