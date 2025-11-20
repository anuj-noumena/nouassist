using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.FundingRounds.Dtos
{
    /// <summary>
    /// DTO for recording an investment in a funding round
    /// </summary>
    public class RecordInvestmentDto
    {
        [Required]
        [Range(0.01, (double)decimal.MaxValue)]
        public decimal InvestmentAmount { get; set; }
        
        [Required]
        [Range(1, (double)CapTableConsts.MaxShareQuantity)]
        public decimal SharesIssued { get; set; }
        
        [StringLength(3)]
        public string Currency { get; set; } = "USD";
        
        [StringLength(CapTableConsts.MaxTransactionMemoLength)]
        public string Memo { get; set; }
    }
}
