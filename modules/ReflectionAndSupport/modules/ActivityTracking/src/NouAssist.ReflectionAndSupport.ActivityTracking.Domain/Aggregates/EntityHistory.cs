using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.Aggregates
{
    public class EntityHistory : CreationAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string EntityType { get; private set; }
        public Guid EntityId { get; private set; }
        public int Version { get; private set; }
        public string Action { get; private set; }
        public Guid ActorId { get; private set; }
        public string ActorName { get; private set; }
        public Dictionary<string, object> PreviousState { get; private set; }
        public Dictionary<string, object> CurrentState { get; private set; }
        public Dictionary<string, object> Changes { get; private set; }
        public string ChangeDescription { get; private set; }
        public string IpAddress { get; private set; }
        public string UserAgent { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }
        public DateTime OccurredAt { get; private set; }
        public Guid? TenantId { get; private set; }

        protected EntityHistory() { /* ORM */ }

        public EntityHistory(
            Guid id,
            string entityType,
            Guid entityId,
            int version,
            string action,
            Guid actorId,
            Guid? tenantId,
            string actorName = null) : base(id)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            EntityId = Check.NotNull(entityId, nameof(entityId));
            Version = Check.Positive(version, nameof(version));
            Action = Check.NotNullOrWhiteSpace(action, nameof(action));
            ActorId = Check.NotNull(actorId, nameof(actorId));
            TenantId = tenantId;
            ActorName = actorName;
            
            OccurredAt = DateTime.UtcNow;
            PreviousState = new Dictionary<string, object>();
            CurrentState = new Dictionary<string, object>();
            Changes = new Dictionary<string, object>();
            Metadata = new Dictionary<string, object>();
        }

        public void SetStates(
            Dictionary<string, object> previousState,
            Dictionary<string, object> currentState)
        {
            PreviousState = previousState ?? new Dictionary<string, object>();
            CurrentState = currentState ?? new Dictionary<string, object>();
        }

        public void RecordChanges(Dictionary<string, object> changes, string description = null)
        {
            if (changes != null && changes.Count > 0)
            {
                Changes = changes;
                ChangeDescription = description;
            }
        }

        public void SetRequestInfo(string ipAddress, string userAgent)
        {
            IpAddress = ipAddress;
            UserAgent = userAgent;
        }

        public void AddMetadata(string key, object value)
        {
            Check.NotNullOrWhiteSpace(key, nameof(key));
            Metadata[key] = value;
        }

        public bool HasChanges => Changes != null && Changes.Count > 0;
        public bool HasPreviousState => PreviousState != null && PreviousState.Count > 0;
        public bool HasCurrentState => CurrentState != null && CurrentState.Count > 0;
        public bool IsCreation => Action.Equals("created", StringComparison.OrdinalIgnoreCase);
        public bool IsUpdate => Action.Equals("updated", StringComparison.OrdinalIgnoreCase);
        public bool IsDeletion => Action.Equals("deleted", StringComparison.OrdinalIgnoreCase);

        public List<string> GetChangedFields()
        {
            if (Changes == null) return new List<string>();
            return new List<string>(Changes.Keys);
        }
    }
}
