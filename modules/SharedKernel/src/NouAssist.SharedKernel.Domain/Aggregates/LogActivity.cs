using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NouAssist.SharedKernel.Aggregates
{
    public class LogActivity : Entity<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Action { get; private set; }
        public string? EntityType { get; private set; }
        public Guid? EntityId { get; private set; }
        public string? Description { get; set; }
        public string? MetadataJson { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime OccurredAt { get; private set; }

        protected LogActivity() { }

        public LogActivity(
            Guid id,
            Guid userId,
            string action,
            string? entityType = null,
            Guid? entityId = null,
            Guid? tenantId = null) : base(id)
        {
            UserId = userId;
            Action = Check.NotNullOrWhiteSpace(action, nameof(action), maxLength: 100);
            EntityType = entityType;
            EntityId = entityId;
            TenantId = tenantId;
            OccurredAt = DateTime.UtcNow;
        }

        public bool HasEntity => !string.IsNullOrWhiteSpace(EntityType) && EntityId.HasValue;
        public bool HasMetadata => !string.IsNullOrWhiteSpace(MetadataJson);
    }
}
