using System;
using System.Collections.Generic;
using NouAssist.Infrastructure.SearchEngine.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.Infrastructure.SearchEngine.ValueObjects
{
    public class SearchSuggestion : ValueObject
    {
        public string Query { get; init; }
        public SuggestionSource Source { get; init; }
        public double Score { get; init; }
        public int? UseCount { get; init; }
        public DateTime? LastUsedAt { get; init; }
        public string Category { get; init; }
        public Dictionary<string, object> Metadata { get; init; }

        private SearchSuggestion() { }

        public SearchSuggestion(
            string query,
            SuggestionSource source,
            double score,
            int? useCount = null,
            DateTime? lastUsedAt = null,
            string category = null,
            Dictionary<string, object> metadata = null)
        {
            Query = query;
            Source = source;
            Score = score;
            UseCount = useCount;
            LastUsedAt = lastUsedAt;
            Category = category;
            Metadata = metadata ?? new Dictionary<string, object>();
        }

        public static SearchSuggestion FromHistory(string query, int useCount, DateTime lastUsedAt)
        {
            var recencyScore = CalculateRecencyScore(lastUsedAt);
            var frequencyScore = CalculateFrequencyScore(useCount);
            var score = (recencyScore * 0.6) + (frequencyScore * 0.4);

            return new SearchSuggestion(
                query,
                SuggestionSource.History,
                score,
                useCount,
                lastUsedAt
            );
        }

        public static SearchSuggestion FromPopular(string query, int useCount)
        {
            var score = CalculateFrequencyScore(useCount);

            return new SearchSuggestion(
                query,
                SuggestionSource.Popular,
                score,
                useCount
            );
        }

        public static SearchSuggestion FromEntity(string query, string category, Dictionary<string, object> metadata = null)
        {
            return new SearchSuggestion(
                query,
                SuggestionSource.Entity,
                0.5, // Default score for entity suggestions
                category: category,
                metadata: metadata
            );
        }

        public bool IsFromHistory => Source == SuggestionSource.History;
        public bool IsFromPopular => Source == SuggestionSource.Popular;
        public bool IsFromEntity => Source == SuggestionSource.Entity;

        private static double CalculateRecencyScore(DateTime lastUsedAt)
        {
            var daysSinceUse = (DateTime.UtcNow - lastUsedAt).Days;

            if (daysSinceUse < 1) return 1.0;
            if (daysSinceUse < 7) return 0.8;
            if (daysSinceUse < 30) return 0.5;
            if (daysSinceUse < 90) return 0.3;
            return 0.1;
        }

        private static double CalculateFrequencyScore(int useCount)
        {
            if (useCount >= 20) return 1.0;
            if (useCount >= 10) return 0.8;
            if (useCount >= 5) return 0.6;
            if (useCount >= 2) return 0.4;
            return 0.2;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Query;
            yield return Source;
            yield return Score;
        }
    }
}
