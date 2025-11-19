using System;
using NouAssist.FinancialManagement.AuditTrail.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AuditTrail.Dtos
{
    public class AuditSearchDto : PagedAndSortedResultRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public Guid? UserId { get; set; }
        public AuditEventType? EventType { get; set; }
        public AuditSeverity? MinimumSeverity { get; set; }
        public ComplianceFlag? ComplianceFlags { get; set; }
        public bool? IncludeArchived { get; set; }
    }
}
