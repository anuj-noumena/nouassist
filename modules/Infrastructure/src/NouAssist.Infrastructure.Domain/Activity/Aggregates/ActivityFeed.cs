using System;
using System.Collections.Generic;
using NouAssist.Infrastructure.Activity.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Infrastructure.Activity.Aggregates
{
    public class ActivityFeed : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid ActorId { get; private set; }
        public ActivityAction Action { get; private set; }
        public ActivityEntityType EntityType { get; private set; }
        public Guid EntityId { get; private set; }
        public string EntityTitle { get; private set; }
        public Guid? TargetUserId { get; private set; }
        public string TargetEntityType { get; private set; }
        public Guid? TargetEntityId { get; private set; }
        public string Description { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }
        public Dictionary<string, object> Changes { get; private set; }
        public Guid? ProjectId { get; private set; }
        public Guid? TeamId { get; private set; }
        public ActivityVisibility Visibility { get; private set; }
        public bool IsRead { get; private set; }
        public DateTime? ReadAt { get; private set; }
        public DateTime OccurredAt { get; private set; }
        public Guid? TenantId { get; private set; }

        protected ActivityFeed() { /* ORM */ }

        public ActivityFeed(
            Guid id,
            Guid actorId,
            ActivityAction action,
            ActivityEntityType entityType,
            Guid entityId,
            Guid? tenantId,
            string entityTitle = null,
            Guid? targetUserId = null,
            string description = null) : base(id)
        {
            ActorId = Check.NotNull(actorId, nameof(actorId));
            Action = action;
            EntityType = entityType;
            EntityId = Check.NotNull(entityId, nameof(entityId));
            TenantId = tenantId;
            EntityTitle = entityTitle;
            TargetUserId = targetUserId;
            Description = description;
            
            OccurredAt = DateTime.UtcNow;
            Visibility = ActivityVisibility.Team;
            IsRead = false;
            
            Metadata = new Dictionary<string, object>();
            Changes = new Dictionary<string, object>();
        }

        public void MarkAsRead()
        {
            if (IsRead) return;
            
            IsRead = true;
            ReadAt = DateTime.UtcNow;
        }

        public void MarkAsUnread()
        {
            IsRead = false;
            ReadAt = null;
        }

        public void SetVisibility(ActivityVisibility visibility)
        {
            Visibility = visibility;
        }

        public void AddMetadata(string key, object value)
        {
            Check.NotNullOrWhiteSpace(key, nameof(key));
            Metadata[key] = value;
        }

        public void RecordChanges(Dictionary<string, object> changes)
        {
            if (changes == null || changes.Count == 0) return;
            
            foreach (var change in changes)
            {
                Changes[change.Key] = change.Value;
            }
        }

        public bool HasTarget => TargetUserId.HasValue;
        public bool HasChanges => Changes != null && Changes.Count > 0;
        public bool IsPublic => Visibility == ActivityVisibility.Public;
        public bool IsPrivate => Visibility == ActivityVisibility.Private;

        public string GetActionDescription()
        {
            var actionStr = Action.ToString().ToLowerInvariant();
            var entityStr = EntityType.ToString().ToLowerInvariant();
            
            if (HasTarget)
            {
                return $"{actionStr} {entityStr} for user";
            }
            
            return $"{actionStr} {entityStr}";
        }
    }
}
