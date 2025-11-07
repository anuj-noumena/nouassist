using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Search.Aggregates
{
    public class SavedSearch : CreationAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public const int MaxNameLength = 200;
        public const int MaxQueryLength = 500;

        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string Query { get; private set; }
        public string Filters { get; private set; }
        public int UsageCount { get; private set; }
        public DateTime? LastUsedAt { get; private set; }
        public Guid? TenantId { get; private set; }

        protected SavedSearch() { /* ORM */ }

        public SavedSearch(
            Guid id,
            Guid userId,
            string name,
            string query,
            Guid? tenantId,
            string filters = null) : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), MaxNameLength);
            Query = Check.NotNullOrWhiteSpace(query, nameof(query), MaxQueryLength);
            TenantId = tenantId;
            Filters = filters;
            UsageCount = 0;
        }

        public void RecordUsage()
        {
            UsageCount++;
            LastUsedAt = DateTime.UtcNow;
        }

        public void UpdateName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), MaxNameLength);
        }

        public void UpdateQuery(string query)
        {
            Query = Check.NotNullOrWhiteSpace(query, nameof(query), MaxQueryLength);
        }

        public void UpdateFilters(string filters)
        {
            Filters = filters;
        }

        public bool HasBeenUsed => UsageCount > 0;
        public bool HasFilters => !string.IsNullOrWhiteSpace(Filters);
        public bool IsPopular => UsageCount >= 10;
    }
}
