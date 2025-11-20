using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.CapTables.Dtos
{
    /// <summary>
    /// DTO for CapTable view operations
    /// </summary>
    public class CapTableDto : FullAuditedEntityDto<Guid>
    {
        public string CompanyName { get; set; }
        public string CompanyIdentifier { get; set; }
        public DateTime AsOfDate { get; set; }
        public decimal TotalAuthorizedShares { get; set; }
        public decimal TotalIssuedShares { get; set; }
        public decimal TotalOutstandingShares { get; set; }
        public bool IsActive { get; set; }
        
        // Calculated fields
        public decimal IssuedPercentage => TotalAuthorizedShares > 0 
            ? Math.Round((TotalIssuedShares / TotalAuthorizedShares) * 100, 2) 
            : 0;
        
        public decimal OutstandingPercentage => TotalAuthorizedShares > 0 
            ? Math.Round((TotalOutstandingShares / TotalAuthorizedShares) * 100, 2) 
            : 0;
    }
}
