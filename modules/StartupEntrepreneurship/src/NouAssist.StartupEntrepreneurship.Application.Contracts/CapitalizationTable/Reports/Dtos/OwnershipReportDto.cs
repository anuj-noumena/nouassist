using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Reports.Dtos
{
    /// <summary>
    /// DTO for ownership report
    /// </summary>
    public class OwnershipReportDto
    {
        public Guid CapTableId { get; set; }
        public string CompanyName { get; set; }
        public DateTime AsOfDate { get; set; }
        public decimal TotalShares { get; set; }
        public bool IncludesOptions { get; set; }
        public List<OwnershipEntryDto> Entries { get; set; } = new();
        
        // Summary statistics
        public decimal TotalOwnershipPercentage => Entries.Sum(e => e.OwnershipPercentage);
        public int TotalHolders => Entries.Count;
    }
    
    /// <summary>
    /// DTO for individual ownership entry
    /// </summary>
    public class OwnershipEntryDto
    {
        public Guid HolderId { get; set; }
        public string HolderName { get; set; }
        public string HolderType { get; set; }
        public decimal Shares { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public decimal VestedShares { get; set; }
        public decimal UnvestedShares { get; set; }
    }
}
