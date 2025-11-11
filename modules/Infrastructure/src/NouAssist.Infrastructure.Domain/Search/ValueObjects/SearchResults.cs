using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Infrastructure.Search.ValueObjects
{
    public class SearchResults : ValueObject
    {
        public string Query { get; init; }
        public List<SearchResultItem> Items { get; init; }
        public int TotalCount { get; init; }
        public int Offset { get; init; }
        public int Limit { get; init; }
        public TimeSpan SearchDuration { get; init; }
        public Dictionary<string, int> Facets { get; init; }
        public List<string> Suggestions { get; init; }

        public SearchResults(
            string query,
            List<SearchResultItem> items,
            int totalCount,
            int offset,
            int limit,
            TimeSpan searchDuration,
            Dictionary<string, int> facets = null,
            List<string> suggestions = null)
        {
            Query = query;
            Items = items ?? new List<SearchResultItem>();
            TotalCount = totalCount;
            Offset = offset;
            Limit = limit;
            SearchDuration = searchDuration;
            Facets = facets ?? new Dictionary<string, int>();
            Suggestions = suggestions ?? new List<string>();
        }

        public bool HasResults => Items != null && Items.Count > 0;
        public bool HasMoreResults => TotalCount > (Offset + Items.Count);
        public int CurrentPage => (Offset / Limit) + 1;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / Limit);
        public bool HasFacets => Facets != null && Facets.Count > 0;
        public bool HasSuggestions => Suggestions != null && Suggestions.Count > 0;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Query;
            yield return TotalCount;
            yield return Offset;
            yield return Limit;
        }
    }
}
