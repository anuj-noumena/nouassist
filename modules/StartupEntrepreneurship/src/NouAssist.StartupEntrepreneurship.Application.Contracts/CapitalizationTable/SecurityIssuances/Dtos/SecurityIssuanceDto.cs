using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances.Dtos
{
    /// <summary>
    /// DTO for SecurityIssuance view operations
    /// </summary>
    public class SecurityIssuanceDto : FullAuditedEntityDto<Guid>
    {
        public Guid CapTableId { get; set; }
        public string SecurityType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime IssuanceDate { get; set; }
        public decimal TotalAuthorized { get; set; }
        public decimal TotalIssued { get; set; }
        public decimal TotalOutstanding { get; set; }
        public decimal IssuePriceValue { get; set; }
        public string IssuePriceCurrency { get; set; }
        public bool IsActive { get; set; }
        public Guid? ShareClassId { get; set; }
        
        // Option details (if applicable)
        public decimal? OptionStrikePrice { get; set; }
        public DateTime? OptionExpiryDate { get; set; }
        public string OptionCurrency { get; set; }
        
        // Calculated fields
        public decimal AvailableShares => TotalAuthorized - TotalIssued;
        public decimal IssuedPercentage => TotalAuthorized > 0 
            ? Math.Round((TotalIssued / TotalAuthorized) * 100, 2) 
            : 0;
        public bool IsExpired => OptionExpiryDate.HasValue && DateTime.UtcNow > OptionExpiryDate.Value;
        
        // Related entities
        public List<ShareClassDto> ShareClasses { get; set; } = new();
    }
}
