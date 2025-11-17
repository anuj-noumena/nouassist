using System;
using System.Collections.Generic;
using NouAssist.AuditTrail.Domain.Shared.Events;
using NouAssist.AuditTrail.Enums;
using NouAssist.FinancialManagement.AuditTrail.Aggregates;
using NouAssist.FinancialManagement.AuditTrail.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.AuditTrail.Services
{
    public class AuditLoggingService : DomainService
    {
        private readonly ComplianceFlagDeterminationService _complianceService;

        public AuditLoggingService(ComplianceFlagDeterminationService complianceService)
        {
            _complianceService = complianceService;
        }

        public AuditEntry CreateAuditEntry(
            Guid? tenantId,
            string entityType,
            string entityId,
            string entityName,
            AuditEventType eventType,
            Guid userId,
            string userName,
            string operationDescription,
            object oldValue,
            object newValue,
            Dictionary<string, object> additionalContext = null,
            string ipAddress = null,
            string userAgent = null,
            string sessionId = null)
        {
            var entityReference = new EntityReference(entityType, entityId, entityName);
            var userContext = new UserContext(userId, userName, ipAddress, userAgent, sessionId);
            
            var metadata = new AuditMetadata(additionalContext ?? new Dictionary<string, object>());
            
            var severity = DetermineSeverity(eventType, entityType);
            var complianceFlags = _complianceService.DetermineComplianceFlags(
                eventType, 
                entityType, 
                newValue);
            
            var retentionDays = DetermineRetentionPeriod(entityType, complianceFlags);

            var auditEntry = new AuditEntry(
                GuidGenerator.Create(),
                tenantId,
                entityReference,
                eventType,
                userContext,
                operationDescription,
                SerializeValue(oldValue),
                SerializeValue(newValue),
                metadata,
                severity,
                complianceFlags,
                retentionDays);

            // Raise domain event
            auditEntry.AddDistributedEvent(new AuditEntryCreatedEto
            {
                AuditEntryId = auditEntry.Id,
                TenantId = tenantId,
                EntityType = entityType,
                EntityId = entityId,
                EventType = eventType,
                Severity = severity,
                Timestamp = auditEntry.Timestamp,
                UserId = userId,
                ComplianceFlags = complianceFlags
            });

            // Check for suspicious activity
            if (complianceFlags.HasFlag(ComplianceFlag.Suspicious))
            {
                RaiseSuspiciousActivityAlert(auditEntry);
            }

            return auditEntry;
        }

        private AuditSeverity DetermineSeverity(AuditEventType eventType, string entityType)
        {
            // Financial entities are always high severity
            if (IsFinancialEntity(entityType))
            {
                return AuditSeverity.High;
            }

            return eventType switch
            {
                AuditEventType.Delete => AuditSeverity.High,
                AuditEventType.Post => AuditSeverity.High,
                AuditEventType.Approve => AuditSeverity.Medium,
                AuditEventType.Update => AuditSeverity.Medium,
                AuditEventType.Create => AuditSeverity.Low,
                AuditEventType.Read => AuditSeverity.Low,
                _ => AuditSeverity.Low
            };
        }

        private int DetermineRetentionPeriod(string entityType, ComplianceFlag complianceFlags)
        {
            // SOX requires 7 years
            if (complianceFlags.HasFlag(ComplianceFlag.SOX))
            {
                return 2555; // 7 years
            }

            // GDPR - typically 6 years
            if (complianceFlags.HasFlag(ComplianceFlag.GDPR))
            {
                return 2190; // 6 years
            }

            // Financial transactions - 7 years
            if (complianceFlags.HasFlag(ComplianceFlag.FinancialTransaction))
            {
                return 2555;
            }

            // Default - 3 years
            return 1095;
        }

        private bool IsFinancialEntity(string entityType)
        {
            var financialEntities = new[] 
            { 
                "Bill", "Invoice", "Payment", "JournalEntry", 
                "BankTransaction", "Expense", "Budget" 
            };
            
            return Array.Exists(financialEntities, e => 
                entityType.Equals(e, StringComparison.OrdinalIgnoreCase));
        }

        private string SerializeValue(object value)
        {
            if (value == null) return null;
            return System.Text.Json.JsonSerializer.Serialize(value);
        }

        private void RaiseSuspiciousActivityAlert(AuditEntry entry)
        {
            entry.AddDistributedEvent(new SuspiciousActivityDetectedEto
            {
                AlertId = GuidGenerator.Create(),
                TenantId = entry.TenantId,
                UserId = entry.UserContext.UserId,
                UserName = entry.UserContext.UserName,
                ActivityDescription = entry.OperationDescription,
                SuspiciousPatterns = new List<string> { "Flagged by compliance engine" },
                DetectedAt = entry.Timestamp,
                Context = entry.Metadata.Properties
            });
        }
    }
}
