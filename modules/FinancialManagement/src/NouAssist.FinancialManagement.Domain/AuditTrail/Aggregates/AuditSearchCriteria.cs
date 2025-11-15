using System;
using NouAssist.AuditTrail.Enums;
using Volo.Abp.Domain.Entities;

namespace NouAssist.AuditTrail.Aggregates
{
    public class AuditSearchCriteria : Entity<Guid>
    {
        public Guid? TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public Guid? UserId { get; set; }
        public AuditEventType? EventType { get; set; }
        public AuditSeverity? MinimumSeverity { get; set; }
        public ComplianceFlag? ComplianceFlags { get; set; }
        public bool? IncludeArchived { get; set; }
        public int MaxResults { get; set; } = 1000;

        protected AuditSearchCriteria() { }

        public AuditSearchCriteria(
            DateTime startDate, 
            DateTime endDate,
            Guid? tenantId = null)
        {
            if ((endDate - startDate).TotalDays > 365)
            {
                throw new BusinessException("AuditTrail:SearchPeriodTooLarge");
            }

            StartDate = startDate;
            EndDate = endDate;
            TenantId = tenantId;
            IncludeArchived = false;
        }

        public void SetEntityFilter(string entityType, string entityId = null)
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        public void SetUserFilter(Guid userId)
        {
            UserId = userId;
        }

        public void SetComplianceFilter(ComplianceFlag flags)
        {
            ComplianceFlags = flags;
        }
    }
}
