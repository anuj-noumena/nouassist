using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Infrastructure.Search.Aggregates
{
    public class PopularSearch : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public const int MaxQueryLength = 500;

        public string Query { get; private set; }
        public int SearchCount { get; private set; }
        public DateTime? LastSearchedAt { get; private set; }
        public Guid? TenantId { get; private set; }

        protected PopularSearch() { /* ORM */ }

        public PopularSearch(
            Guid id,
            string query,
            Guid? tenantId) : base(id)
        {
            Query = Check.NotNullOrWhiteSpace(query, nameof(query), MaxQueryLength);
            TenantId = tenantId;
            SearchCount = 1;
            LastSearchedAt = DateTime.UtcNow;
        }

        public void IncrementCount()
        {
            SearchCount++;
            LastSearchedAt = DateTime.UtcNow;
        }

        public bool IsVeryPopular => SearchCount > 100;
        public bool IsPopular => SearchCount > 50;
        public bool IsRecentlySearched => LastSearchedAt.HasValue && 
                                          (DateTime.UtcNow - LastSearchedAt.Value).Days < 7;
    }
}
