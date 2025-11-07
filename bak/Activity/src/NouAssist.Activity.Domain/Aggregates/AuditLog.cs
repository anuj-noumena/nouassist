using System;
using System.Collections.Generic;
using NouAssist.Activity.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Activity.Aggregates
{
    public class AuditLog : CreationAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public string UserName { get; private set; }
        public AuditAction Action { get; private set; }
        public AuditCategory Category { get; private set; }
        public string EntityType { get; private set; }
        public Guid? EntityId { get; private set; }
        public string EntityTitle { get; private set; }
        public string Description { get; private set; }
        public AuditSeverity Severity { get; private set; }
        public string IpAddress { get; private set; }
        public string UserAgent { get; private set; }
        public string DeviceId { get; private set; }
        public string SessionId { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }
        public Dictionary<string, object> ChangedFields { get; private set; }
        public string ErrorMessage { get; private set; }
        public DateTime Timestamp { get; private set; }
        public Guid? TenantId { get; private set; }

        protected AuditLog() { /* ORM */ }

        public AuditLog(
            Guid id,
            Guid userId,
            AuditAction action,
            AuditCategory category,
            Guid? tenantId,
            string userName = null,
            string entityType = null,
            Guid? entityId = null,
            string description = null,
            AuditSeverity severity = AuditSeverity.Info,
            string ipAddress = null) : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            Action = action;
            Category = category;
            TenantId = tenantId;
            UserName = userName;
            EntityType = entityType;
            EntityId = entityId;
            Description = description;
            Severity = severity;
            IpAddress = ipAddress;
            
            Timestamp = DateTime.UtcNow;
            Metadata = new Dictionary<string, object>();
            ChangedFields = new Dictionary<string, object>();
        }

        public void SetError(string errorMessage)
        {
            ErrorMessage = Check.NotNullOrWhiteSpace(errorMessage, nameof(errorMessage));
            Severity = AuditSeverity.Error;
        }

        public void SetDeviceInfo(string deviceId, string userAgent)
        {
            DeviceId = deviceId;
            UserAgent = userAgent;
        }

        public void SetSessionId(string sessionId)
        {
            SessionId = sessionId;
        }

        public void AddMetadata(string key, object value)
        {
            Check.NotNullOrWhiteSpace(key, nameof(key));
            Metadata[key] = value;
        }

        public void RecordFieldChanges(Dictionary<string, object> changes)
        {
            if (changes == null || changes.Count == 0) return;
            
            foreach (var change in changes)
            {
                ChangedFields[change.Key] = change.Value;
            }
        }

        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool IsCritical => Severity == AuditSeverity.Critical;
        public bool IsWarning => Severity == AuditSeverity.Warning;
        public bool HasChangedFields => ChangedFields != null && ChangedFields.Count > 0;
    }
}
