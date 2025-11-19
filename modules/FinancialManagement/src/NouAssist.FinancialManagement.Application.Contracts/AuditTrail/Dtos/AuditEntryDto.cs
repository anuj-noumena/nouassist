using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.AuditTrail.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AuditTrail.Dtos
{
    public class AuditEntryDto : EntityDto<Guid>
    {
        public DateTime Timestamp { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public AuditEventType EventType { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string OperationDescription { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public AuditSeverity Severity { get; set; }
        public ComplianceFlag ComplianceFlags { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? ArchivedAt { get; set; }
    }
}
