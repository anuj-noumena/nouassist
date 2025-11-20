using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools.Dtos
{
    /// <summary>
    /// DTO for granting options from a pool
    /// </summary>
    public class GrantOptionsDto
    {
        [Required]
        public Guid ToHolderId { get; set; }
        
        [Required]
        public Guid SecurityIssuanceId { get; set; }
        
        [Required]
        [Range(1, (double)CapTableConsts.MaxShareQuantity)]
        public decimal Quantity { get; set; }
        
        [Required]
        [Range((double)CapTableConsts.MinPricePerShare, (double)CapTableConsts.MaxPricePerShare)]
        public decimal StrikePrice { get; set; }
        
        [StringLength(3)]
        public string Currency { get; set; } = "USD";
        
        public DateTime? GrantDate { get; set; }
        
        public DateTime? ExpiryDate { get; set; }
        
        // Custom vesting terms (optional - will use pool defaults if not provided)
        [Range(1, int.MaxValue)]
        public int? VestingPeriodMonths { get; set; }
        
        [Range(0, int.MaxValue)]
        public int? CliffMonths { get; set; }
        
        [Range(0.01, 100)]
        public decimal? VestingPercentage { get; set; }
        
        [StringLength(CapTableConsts.MaxTransactionMemoLength)]
        public string Memo { get; set; }
    }
}
