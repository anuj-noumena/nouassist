using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos
{
    public class UpdateTermSheetOfferDto
    {
        [Required]
        [StringLength(FundraisingPipelineConsts.TermSheet.MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        public DateTime ReceivedDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public Guid? FundraisingRoundId { get; set; }

        [Required]
        public UpdateTermComparisonDataDto TermData { get; set; }
    }

    public class UpdateTermComparisonDataDto
    {
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Pre-money valuation must be greater than zero.")]
        public decimal PreMoneyValuation { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Investment amount must be greater than zero.")]
        public decimal InvestmentAmount { get; set; }

        public bool HasLiquidationPreference { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Liquidation multiple must be at least 1.")]
        public decimal LiquidationMultiple { get; set; } = 1;

        public bool HasParticipationRights { get; set; }

        public bool HasProRataRights { get; set; }

        public bool HasAntiDilutionProtection { get; set; }

        public Dictionary<string, bool> InvestorRights { get; set; }
    }
}
