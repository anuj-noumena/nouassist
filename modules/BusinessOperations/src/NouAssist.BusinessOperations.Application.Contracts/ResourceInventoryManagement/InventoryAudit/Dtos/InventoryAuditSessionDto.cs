using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos
{
    /// <summary>
    /// DTO for inventory audit session information
    /// </summary>
    public class InventoryAuditSessionDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// ID of the storage location being audited
        /// </summary>
        public Guid LocationId { get; set; }
        
        /// <summary>
        /// Code of the storage location
        /// </summary>
        public string LocationCode { get; set; } = null!;
        
        /// <summary>
        /// Name of the storage location
        /// </summary>
        public string LocationName { get; set; } = null!;

        /// <summary>
        /// Name of the audit session
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description or purpose of the audit
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Current status of the audit
        /// </summary>
        public AuditStatus Status { get; set; }

        /// <summary>
        /// Scheduled start time for the audit
        /// </summary>
        public DateTime ScheduledStartTime { get; set; }

        /// <summary>
        /// Actual start time of the audit
        /// </summary>
        public DateTime? ActualStartTime { get; set; }

        /// <summary>
        /// Completion time of the audit
        /// </summary>
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// ID of the user conducting the audit
        /// </summary>
        public Guid AuditedByUserId { get; set; }
        
        /// <summary>
        /// Name of the user conducting the audit
        /// </summary>
        public string? AuditedByUserName { get; set; }

        /// <summary>
        /// Notes about the audit session
        /// </summary>
        public string? Notes { get; set; }
        
        /// <summary>
        /// Number of items audited
        /// </summary>
        public int ItemsAudited { get; set; }
        
        /// <summary>
        /// Number of discrepancies found
        /// </summary>
        public int DiscrepanciesFound { get; set; }
        
        /// <summary>
        /// List of audit discrepancies
        /// </summary>
        public List<AuditDiscrepancyDto> Discrepancies { get; set; } = new();
    }
}
