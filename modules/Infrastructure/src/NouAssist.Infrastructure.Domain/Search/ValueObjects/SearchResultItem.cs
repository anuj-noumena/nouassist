using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Infrastructure.Search.ValueObjects
{
    public class SearchResultItem : ValueObject
    {
        public Guid Id { get; init; }
        public string EntityType { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Snippet { get; init; }
        public Dictionary<string, object> Metadata { get; init; }
        public double? RelevanceScore { get; init; }
        public DateTime? CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public string ThumbnailUrl { get; init; }
        public List<string> Tags { get; init; }

        public SearchResultItem(
            Guid id,
            string entityType,
            string title,
            string description = null,
            string snippet = null,
            Dictionary<string, object> metadata = null,
            double? relevanceScore = null,
            DateTime? createdAt = null,
            DateTime? updatedAt = null,
            string thumbnailUrl = null,
            List<string> tags = null)
        {
            Id = id;
            EntityType = entityType;
            Title = title;
            Description = description;
            Snippet = snippet;
            Metadata = metadata ?? new Dictionary<string, object>();
            RelevanceScore = relevanceScore;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            ThumbnailUrl = thumbnailUrl;
            Tags = tags ?? new List<string>();
        }

        public bool HasSnippet => !string.IsNullOrWhiteSpace(Snippet);
        public bool HasThumbnail => !string.IsNullOrWhiteSpace(ThumbnailUrl);
        public bool HasTags => Tags != null && Tags.Count > 0;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return EntityType;
            yield return Title;
            yield return RelevanceScore;
        }
    }
}
