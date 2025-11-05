using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Search.Aggregates;
using NouAssist.Search.Data;
using NouAssist.Search.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.Search.Services
{
    public class SearchSuggestionService : DomainService
    {
        private readonly ISearchHistoryRepository _historyRepository;
        private readonly IPopularSearchRepository _popularSearchRepository;

        public SearchSuggestionService(
            ISearchHistoryRepository historyRepository,
            IPopularSearchRepository popularSearchRepository)
        {
            _historyRepository = historyRepository;
            _popularSearchRepository = popularSearchRepository;
        }

        public async Task<List<SearchSuggestion>> GetSuggestionsAsync(
            Guid userId,
            string partialQuery,
            int maxResults = 10)
        {
            var suggestions = new List<SearchSuggestion>();

            // Get suggestions from user's search history
            var historySuggestions = await GetHistorySuggestionsAsync(userId, partialQuery, maxResults / 2);
            suggestions.AddRange(historySuggestions);

            // Get suggestions from popular searches
            var popularSuggestions = await GetPopularSuggestionsAsync(partialQuery, maxResults / 2);
            suggestions.AddRange(popularSuggestions);

            // Sort by score and return top results
            return suggestions
                .OrderByDescending(s => s.Score)
                .Take(maxResults)
                .ToList();
        }

        private async Task<List<SearchSuggestion>> GetHistorySuggestionsAsync(
            Guid userId,
            string partialQuery,
            int maxResults)
        {
            var recentSearches = await _historyRepository.GetRecentByUserAsync(userId, 100);

            var matchingSearches = recentSearches
                .Where(h => h.Query.Contains(partialQuery, StringComparison.OrdinalIgnoreCase))
                .GroupBy(h => h.Query.ToLowerInvariant())
                .Select(g => new
                {
                    Query = g.First().Query,
                    UseCount = g.Count(),
                    LastUsedAt = g.Max(h => h.SearchedAt)
                })
                .Take(maxResults);

            return matchingSearches
                .Select(s => SearchSuggestion.FromHistory(s.Query, s.UseCount, s.LastUsedAt))
                .ToList();
        }

        private async Task<List<SearchSuggestion>> GetPopularSuggestionsAsync(
            string partialQuery,
            int maxResults)
        {
            var popularSearches = await _popularSearchRepository.GetTopSearchesAsync(100);

            var matchingSearches = popularSearches
                .Where(p => p.Query.Contains(partialQuery, StringComparison.OrdinalIgnoreCase))
                .Take(maxResults);

            return matchingSearches
                .Select(p => SearchSuggestion.FromPopular(p.Query, p.SearchCount))
                .ToList();
        }

        public List<string> GetQueryCompletions(string partialQuery, List<string> allQueries, int maxResults = 5)
        {
            if (string.IsNullOrWhiteSpace(partialQuery))
                return new List<string>();

            return allQueries
                .Where(q => q.StartsWith(partialQuery, StringComparison.OrdinalIgnoreCase))
                .Take(maxResults)
                .ToList();
        }

        public List<string> GetRelatedQueries(string query, List<string> allQueries, int maxResults = 5)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<string>();

            var queryTerms = query.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return allQueries
                .Where(q => q != query && queryTerms.Any(term => 
                    q.Contains(term, StringComparison.OrdinalIgnoreCase)))
                .Take(maxResults)
                .ToList();
        }
    }
}
