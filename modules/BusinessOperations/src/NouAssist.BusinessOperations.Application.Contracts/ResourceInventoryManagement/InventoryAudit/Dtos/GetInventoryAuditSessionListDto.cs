using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos
{
    /// <summary>
    /// DTO for getting a list of inventory audit sessions with filtering and sorting
    /// </summary>
    public class GetInventoryAuditSessionListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to filter audit sessions by name or description
        /// </summary>
        public string? Filter { get; set; }
        
        /// <summary>
        /// Filter by storage location ID
        /// </summary>
        public Guid? LocationId { get; set; }
        
        /// <summary>
        /// Filter by audit status
        /// </summary>
        public AuditStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by audited by user ID
        /// </summary>
        public Guid? AuditedByUserId { get; set; }
        
        /// <summary>
        /// Filter by scheduled start date (from)
        /// </summary>
        public DateTime? ScheduledStartFrom { get; set; }
        
        /// <summary>
        /// Filter by scheduled start date (to)
        /// </summary>
        public DateTime? ScheduledStartTo { get; set; }
        
        /// <summary>
        /// Whether to include discrepancies in results
        /// </summary>
        public bool IncludeDiscrepancies { get; set; }
    }
}
