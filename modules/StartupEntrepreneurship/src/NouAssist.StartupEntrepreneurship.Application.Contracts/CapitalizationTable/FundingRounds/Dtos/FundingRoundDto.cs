using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.FundingRounds.Dtos
{
    /// <summary>
    /// DTO for FundingRound view operations
    /// </summary>
    public class FundingRoundDto : FullAuditedEntityDto<Guid>
    {
        public Guid CapTableId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string LeadInvestor { get; set; }
        
        // Valuation
        public decimal PreMoneyValuation { get; set; }
        public decimal PostMoneyValuation { get; set; }
        public string ValuationCurrency { get; set; }
        
        // Investment amounts
        public decimal TargetAmount { get; set; }
        public decimal RaisedAmount { get; set; }
        public string Currency { get; set; }
        
        // Share details
        public decimal SharesIssued { get; set; }
        public decimal PricePerShare { get; set; }
        
        // Calculated fields
        public decimal CompletionPercentage => TargetAmount > 0 
            ? Math.Round((RaisedAmount / TargetAmount) * 100, 2) 
            : 0;
        public decimal RemainingAmount => Math.Max(TargetAmount - RaisedAmount, 0);
        public bool IsCompleted => CompletionPercentage >= 100;
    }
}
