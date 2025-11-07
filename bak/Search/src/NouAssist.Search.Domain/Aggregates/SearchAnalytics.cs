using System;
using System.Collections.Generic;
using NouAssist.Search.Enums;
using NouAssist.Search.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Search.Aggregates
{
    public class SearchAnalytics : AggregateRoot<Guid>, IMultiTenant
    {
        public const int MaxQueryLength = 500;

        public Guid UserId { get; private set; }
        public string Query { get; private set; }
        public List<SearchableEntityType> EntityTypes { get; private set; }
        public Dictionary<string, object> Filters { get; private set; }
        public int ResultCount { get; private set; }
        public Guid? SelectedResultId { get; private set; }
        public int? SelectedResultPosition { get; private set; }
        public int? SearchDurationMs { get; private set; }
        public double QualityScore { get; private set; }
        public string SessionId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public Guid? TenantId { get; private set; }

        protected SearchAnalytics() { /* ORM */ }

        public SearchAnalytics(
            Guid id,
            Guid userId,
            string query,
            int resultCount,
            double qualityScore,
            Guid? tenantId,
            List<SearchableEntityType> entityTypes = null,
            Dictionary<string, object> filters = null,
            Guid? selectedResultId = null,
            int? selectedResultPosition = null,
            int? searchDurationMs = null,
            string sessionId = null) : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            Query = Check.NotNullOrWhiteSpace(query, nameof(query), MaxQueryLength);
            ResultCount = resultCount;
            QualityScore = qualityScore;
            TenantId = tenantId;
            EntityTypes = entityTypes ?? new List<SearchableEntityType>();
            Filters = filters ?? new Dictionary<string, object>();
            SelectedResultId = selectedResultId;
            SelectedResultPosition = selectedResultPosition;
            SearchDurationMs = searchDurationMs;
            SessionId = sessionId;
            Timestamp = DateTime.UtcNow;

            ValidateQualityScore();

            // Publish analytics event
            AddDistributedEvent(new SearchPerformedEto
            {
                UserId = UserId,
                Query = Query,
                ResultCount = ResultCount,
                SearchDurationMs = SearchDurationMs ?? 0,
                QualityScore = QualityScore,
                TenantId = TenantId,
                SearchedAt = Timestamp
            });
        }

        public void RecordResultSelection(Guid resultId, int position)
        {
            SelectedResultId = resultId;
            SelectedResultPosition = position;
        }

        private void ValidateQualityScore()
        {
            if (QualityScore < 0.0 || QualityScore > 1.0)
            {
                throw new BusinessException("Search:InvalidQualityScore", 
                    "Quality score must be between 0.0 and 1.0.");
            }
        }

        public bool HadResults => ResultCount > 0;
        public bool UserClickedResult => SelectedResultId.HasValue;
        public bool IsHighQuality => QualityScore >= 0.7;
        public bool IsFastSearch => SearchDurationMs.HasValue && SearchDurationMs.Value < 500;
        public bool HasFilters => Filters != null && Filters.Count > 0;
    }
}
