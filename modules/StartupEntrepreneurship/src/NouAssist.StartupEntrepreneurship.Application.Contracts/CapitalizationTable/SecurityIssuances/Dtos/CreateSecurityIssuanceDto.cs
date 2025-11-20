using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances.Dtos
{
    /// <summary>
    /// DTO for creating a new SecurityIssuance (base class)
    /// </summary>
    public abstract class CreateSecurityIssuanceDto
    {
        [Required]
        public Guid CapTableId { get; set; }
        
        [Required]
        [StringLength(CapTableConsts.MaxNameLength)]
        public string Name { get; set; }
        
        [StringLength(CapTableConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        [Required]
        [Range(1, (double)CapTableConsts.MaxShareQuantity)]
        public decimal TotalAuthorized { get; set; }
        
        public DateTime? IssuanceDate { get; set; }
    }
    
    /// <summary>
    /// DTO for creating a stock issuance
    /// </summary>
    public class CreateStockIssuanceDto : CreateSecurityIssuanceDto
    {
        [Required]
        [Range((double)CapTableConsts.MinPricePerShare, (double)CapTableConsts.MaxPricePerShare)]
        public decimal IssuePrice { get; set; }
        
        [StringLength(3)]
        public string Currency { get; set; } = "USD";
    }
    
    /// <summary>
    /// DTO for creating an option issuance
    /// </summary>
    public class CreateOptionIssuanceDto : CreateSecurityIssuanceDto
    {
        [Required]
        [Range((double)CapTableConsts.MinPricePerShare, (double)CapTableConsts.MaxPricePerShare)]
        public decimal StrikePrice { get; set; }
        
        [StringLength(3)]
        public string Currency { get; set; } = "USD";
        
        public DateTime? ExpiryDate { get; set; }
    }
}
