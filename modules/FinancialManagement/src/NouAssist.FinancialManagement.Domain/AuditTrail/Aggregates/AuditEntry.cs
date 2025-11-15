using System;
using System.Collections.Generic;
using NouAssist.AuditTrail.Enums;
using NouAssist.AuditTrail.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NouAssist.AuditTrail.Aggregates
{
    public class AuditEntry : AggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public EntityReference EntityReference { get; private set; }
        public AuditEventType EventType { get; private set; }
        public UserContext UserContext { get; private set; }
        public string OperationDescription { get; private set; }
        public string OldValue { get; private set; }
        public string NewValue { get; private set; }
        public AuditMetadata Metadata { get; private set; }
        public AuditSeverity Severity { get; private set; }
        public ComplianceFlag ComplianceFlags { get; private set; }
        public RetentionPolicy RetentionPolicy { get; private set; }
        public bool IsArchived { get; private set; }
        public DateTime? ArchivedAt { get; private set; }

        protected AuditEntry() { }

        public AuditEntry(
            Guid id,
            Guid? tenantId,
            EntityReference entityReference,
            AuditEventType eventType,
            UserContext userContext,
            string operationDescription,
            string oldValue,
            string newValue,
            AuditMetadata metadata,
            AuditSeverity severity,
            ComplianceFlag complianceFlags,
            int retentionDays = 2555) : base(id) // Default 7 years
        {
            TenantId = tenantId;
            Timestamp = Clock.Now;
            EntityReference = Check.NotNull(entityReference, nameof(entityReference));
            EventType = eventType;
            UserContext = Check.NotNull(userContext, nameof(userContext));
            OperationDescription = Check.NotNullOrWhiteSpace(operationDescription, nameof(operationDescription));
            OldValue = oldValue;
            NewValue = newValue;
            Metadata = metadata ?? new AuditMetadata(new Dictionary<string, object>());
            Severity = severity;
            ComplianceFlags = complianceFlags;
            RetentionPolicy = new RetentionPolicy(retentionDays, Timestamp);
            IsArchived = false;
        }

        public void Archive(string archiveLocation)
        {
            if (IsArchived)
            {
                throw new BusinessException("AuditTrail:AlreadyArchived", "Entry is already archived.");
            }

            IsArchived = true;
            ArchivedAt = Clock.Now;
            RetentionPolicy = new RetentionPolicy(
                RetentionPolicy.RetentionDays,
                Timestamp,
                true,
                archiveLocation);
        }

        public bool ShouldBeArchived(DateTime currentDate)
        {
            return !IsArchived && 
                   RetentionPolicy.IsArchivable && 
                   currentDate >= Timestamp.AddDays(365); // Archive after 1 year
        }

        public bool ShouldBeDeleted(DateTime currentDate)
        {
            return RetentionPolicy.IsExpired(currentDate);
        }
    }
}
