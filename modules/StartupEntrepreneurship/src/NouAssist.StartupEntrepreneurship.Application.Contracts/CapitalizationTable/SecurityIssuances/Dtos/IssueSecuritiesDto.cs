using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances.Dtos
{
    /// <summary>
    /// DTO for issuing securities to a holder
    /// </summary>
    public class IssueSecuritiesDto
    {
        [Required]
        public Guid ToHolderId { get; set; }
        
        [Required]
        public Guid ShareClassId { get; set; }
        
        [Required]
        [Range(1, (double)CapTableConsts.MaxShareQuantity)]
        public decimal Quantity { get; set; }
        
        [Required]
        [Range((double)CapTableConsts.MinPricePerShare, (double)CapTableConsts.MaxPricePerShare)]
        public decimal PricePerShare { get; set; }
        
        [StringLength(3)]
        public string Currency { get; set; } = "USD";
        
        public DateTime? TransactionDate { get; set; }
        
        [StringLength(CapTableConsts.MaxTransactionMemoLength)]
        public string Memo { get; set; }
    }
}
