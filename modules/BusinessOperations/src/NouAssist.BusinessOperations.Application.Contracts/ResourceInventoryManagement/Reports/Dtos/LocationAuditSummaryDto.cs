using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for location audit summary
    /// </summary>
    public class LocationAuditSummaryDto
    {
        /// <summary>
        /// Location ID
        /// </summary>
        public Guid LocationId { get; set; }
        
        /// <summary>
        /// Location code
        /// </summary>
        public string LocationCode { get; set; } = null!;
        
        /// <summary>
        /// Location name
        /// </summary>
        public string LocationName { get; set; } = null!;
        
        /// <summary>
        /// Number of audit sessions
        /// </summary>
        public int AuditCount { get; set; }
        
        /// <summary>
        /// Number of audits with discrepancies
        /// </summary>
        public int AuditsWithDiscrepanciesCount { get; set; }
        
        /// <summary>
        /// Total number of discrepancies found
        /// </summary>
        public int TotalDiscrepancyCount { get; set; }
        
        /// <summary>
        /// Last audit date
        /// </summary>
        public DateTime? LastAuditDate { get; set; }
    }
}
