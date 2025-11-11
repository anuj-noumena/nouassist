using System;
using System.Collections.Generic;
using NouAssist.Infrastructure.Activity.Aggregates;
using NouAssist.Infrastructure.Activity.Enums;
using NouAssist.Infrastructure.Activity.Events;
using Volo.Abp.Domain.Services;

namespace NouAssist.Infrastructure.Activity.Services
{
    public class ActivityTrackingService : DomainService
    {
        public ActivityFeed CreateActivity(
            Guid actorId,
            ActivityAction action,
            ActivityEntityType entityType,
            Guid entityId,
            Guid? tenantId,
            string entityTitle = null,
            Dictionary<string, object> metadata = null,
            Dictionary<string, object> changes = null)
        {
            var activity = new ActivityFeed(
                GuidGenerator.Create(),
                actorId,
                action,
                entityType,
                entityId,
                tenantId,
                entityTitle
            );

            if (metadata != null && metadata.Count > 0)
            {
                foreach (var item in metadata)
                {
                    activity.AddMetadata(item.Key, item.Value);
                }
            }

            if (changes != null && changes.Count > 0)
            {
                activity.RecordChanges(changes);
            }

            // Publish domain event
            activity.AddDistributedEvent(new ActivityCreatedEto
            {
                ActivityId = activity.Id,
                ActorId = activity.ActorId,
                Action = activity.Action.ToString(),
                EntityType = activity.EntityType.ToString(),
                EntityId = activity.EntityId,
                TenantId = activity.TenantId,
                OccurredAt = activity.OccurredAt
            });

            return activity;
        }

        public AuditLog CreateAuditLog(
            Guid userId,
            AuditAction action,
            AuditCategory category,
            Guid? tenantId,
            string userName = null,
            string entityType = null,
            Guid? entityId = null,
            string description = null,
            AuditSeverity severity = AuditSeverity.Info,
            string ipAddress = null,
            Dictionary<string, object> metadata = null)
        {
            var auditLog = new AuditLog(
                GuidGenerator.Create(),
                userId,
                action,
                category,
                tenantId,
                userName,
                entityType,
                entityId,
                description,
                severity,
                ipAddress
            );

            if (metadata != null && metadata.Count > 0)
            {
                foreach (var item in metadata)
                {
                    auditLog.AddMetadata(item.Key, item.Value);
                }
            }

            // Publish domain event
            auditLog.AddDistributedEvent(new AuditLogCreatedEto
            {
                AuditLogId = auditLog.Id,
                UserId = auditLog.UserId,
                Action = auditLog.Action.ToString(),
                Category = auditLog.Category.ToString(),
                Severity = auditLog.Severity.ToString(),
                TenantId = auditLog.TenantId,
                Timestamp = auditLog.Timestamp
            });

            return auditLog;
        }

        public EntityHistory CreateEntityHistory(
            string entityType,
            Guid entityId,
            int version,
            string action,
            Guid actorId,
            Guid? tenantId,
            string actorName = null,
            Dictionary<string, object> previousState = null,
            Dictionary<string, object> currentState = null,
            Dictionary<string, object> changes = null,
            string changeDescription = null)
        {
            var history = new EntityHistory(
                GuidGenerator.Create(),
                entityType,
                entityId,
                version,
                action,
                actorId,
                tenantId,
                actorName
            );

            history.SetStates(previousState, currentState);
            
            if (changes != null && changes.Count > 0)
            {
                history.RecordChanges(changes, changeDescription);
            }

            return history;
        }
    }
}
