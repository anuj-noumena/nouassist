using System;
using System.Collections.Generic;

namespace NouAssist.Infrastructure.Search.Search.Dtos
{
    public class SearchResultsDto
    {
        public string Query { get; set; }
        public List<SearchResultItemDto> Items { get; set; }
        public int TotalCount { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int SearchDurationMs { get; set; }
        public Dictionary<string, int> Facets { get; set; }
        public List<string> Suggestions { get; set; }
        public bool HasResults { get; set; }
        public bool HasMoreResults { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

    public class SearchResultItemDto
    {
        public Guid Id { get; set; }
        public string EntityType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Snippet { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public double? RelevanceScore { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<string> Tags { get; set; }
    }
}
