using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for audit summary report
    /// </summary>
    public class AuditSummaryReportDto
    {
        /// <summary>
        /// Start date of the report period
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the report period
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Location ID filter (if any)
        /// </summary>
        public Guid? LocationId { get; set; }
        
        /// <summary>
        /// Location name (if filtered by location)
        /// </summary>
        public string? LocationName { get; set; }
        
        /// <summary>
        /// Total number of audit sessions in the report
        /// </summary>
        public int TotalAuditCount { get; set; }
        
        /// <summary>
        /// Number of audits with discrepancies
        /// </summary>
        public int AuditsWithDiscrepanciesCount { get; set; }
        
        /// <summary>
        /// Total number of discrepancies found
        /// </summary>
        public int TotalDiscrepancyCount { get; set; }
        
        /// <summary>
        /// Summary of discrepancies by item
        /// </summary>
        public List<ItemDiscrepancySummaryDto> ItemDiscrepancies { get; set; } = new();
        
        /// <summary>
        /// Summary of audits by location
        /// </summary>
        public List<LocationAuditSummaryDto> LocationAudits { get; set; } = new();
    }
}
