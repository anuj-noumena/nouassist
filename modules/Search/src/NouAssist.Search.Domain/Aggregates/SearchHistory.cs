using System;
using System.Collections.Generic;
using NouAssist.Search.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Search.Aggregates
{
    public class SearchHistory : CreationAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public const int MaxQueryLength = 500;

        public Guid UserId { get; private set; }
        public string Query { get; private set; }
        public int? ResultCount { get; private set; }
        public List<SearchableEntityType> EntityTypes { get; private set; }
        public Dictionary<string, object> Filters { get; private set; }
        public DateTime SearchedAt { get; private set; }
        public Guid? TenantId { get; private set; }

        protected SearchHistory() { /* ORM */ }

        public SearchHistory(
            Guid id,
            Guid userId,
            string query,
            Guid? tenantId,
            int? resultCount = null,
            List<SearchableEntityType> entityTypes = null,
            Dictionary<string, object> filters = null) : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            Query = Check.NotNullOrWhiteSpace(query, nameof(query), MaxQueryLength);
            TenantId = tenantId;
            ResultCount = resultCount;
            EntityTypes = entityTypes ?? new List<SearchableEntityType>();
            Filters = filters ?? new Dictionary<string, object>();
            SearchedAt = DateTime.UtcNow;
        }

        public void UpdateResultCount(int resultCount)
        {
            ResultCount = resultCount;
        }

        public bool HadResults => ResultCount.HasValue && ResultCount.Value > 0;
        public bool IsRecent => (DateTime.UtcNow - SearchedAt).Hours < 24;
        public bool HasFilters => Filters != null && Filters.Count > 0;
        public bool HasEntityTypeFilter => EntityTypes != null && EntityTypes.Count > 0;
    }
}
