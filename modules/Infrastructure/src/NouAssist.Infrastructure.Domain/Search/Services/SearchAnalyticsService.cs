using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Search.Aggregates;
using NouAssist.Search.Data;
using NouAssist.Search.Enums;
using NouAssist.Search.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.Search.Services
{
    public class SearchAnalyticsService : DomainService
    {
        private readonly ISearchAnalyticsRepository _analyticsRepository;
        private readonly ISearchHistoryRepository _historyRepository;
        private readonly IPopularSearchRepository _popularSearchRepository;

        public SearchAnalyticsService(
            ISearchAnalyticsRepository analyticsRepository,
            ISearchHistoryRepository historyRepository,
            IPopularSearchRepository popularSearchRepository)
        {
            _analyticsRepository = analyticsRepository;
            _historyRepository = historyRepository;
            _popularSearchRepository = popularSearchRepository;
        }

        public async Task<SearchAnalytics> RecordSearchAsync(
            Guid userId,
            string query,
            SearchResults results,
            int searchDurationMs,
            Guid? tenantId,
            List<SearchableEntityType> entityTypes = null,
            Dictionary<string, object> filters = null,
            string sessionId = null)
        {
            // Calculate quality score
            var qualityScore = CalculateQualityScore(results, searchDurationMs);

            // Create analytics record
            var analytics = new SearchAnalytics(
                GuidGenerator.Create(),
                userId,
                query,
                results.TotalCount,
                qualityScore,
                tenantId,
                entityTypes,
                filters,
                null,
                null,
                searchDurationMs,
                sessionId
            );

            // Record in search history
            await RecordSearchHistoryAsync(userId, query, results.TotalCount, tenantId, entityTypes, filters);

            // Update popular searches
            await UpdatePopularSearchAsync(query, tenantId);

            return analytics;
        }

        private async Task RecordSearchHistoryAsync(
            Guid userId,
            string query,
            int resultCount,
            Guid? tenantId,
            List<SearchableEntityType> entityTypes,
            Dictionary<string, object> filters)
        {
            var history = new SearchHistory(
                GuidGenerator.Create(),
                userId,
                query,
                tenantId,
                resultCount,
                entityTypes,
                filters
            );

            await _historyRepository.InsertAsync(history);
        }

        private async Task UpdatePopularSearchAsync(string query, Guid? tenantId)
        {
            var popularSearch = await _popularSearchRepository.FindByQueryAsync(query);

            if (popularSearch == null)
            {
                popularSearch = new PopularSearch(
                    GuidGenerator.Create(),
                    query,
                    tenantId
                );
                await _popularSearchRepository.InsertAsync(popularSearch);
            }
            else
            {
                popularSearch.IncrementCount();
                await _popularSearchRepository.UpdateAsync(popularSearch);
            }
        }

        private double CalculateQualityScore(SearchResults results, int searchDurationMs)
        {
            double score = 0.0;

            // Factor 1: Result count (40%)
            if (results.TotalCount > 0)
            {
                score += 0.4;
                
                // Bonus for optimal result count (5-50 results)
                if (results.TotalCount >= 5 && results.TotalCount <= 50)
                {
                    score += 0.1;
                }
            }

            // Factor 2: Search speed (30%)
            if (searchDurationMs < 100)
            {
                score += 0.3;
            }
            else if (searchDurationMs < 500)
            {
                score += 0.2;
            }
            else if (searchDurationMs < 1000)
            {
                score += 0.1;
            }

            // Factor 3: Result relevance (20%)
            // Assuming first page results indicate relevance
            if (results.HasResults && results.Items.Any(i => i.RelevanceScore.HasValue))
            {
                var avgRelevance = results.Items
                    .Where(i => i.RelevanceScore.HasValue)
                    .Average(i => i.RelevanceScore.Value);
                
                score += avgRelevance * 0.2;
            }

            // Factor 4: Suggestions quality (10%)
            if (results.HasSuggestions)
            {
                score += 0.1;
            }

            return Math.Min(score, 1.0);
        }

        public async Task<Dictionary<string, object>> GetSearchMetricsAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var analytics = await _analyticsRepository.GetByDateRangeAsync(userId, startDate, endDate);

            return new Dictionary<string, object>
            {
                { "totalSearches", analytics.Count },
                { "averageResultCount", analytics.Any() ? analytics.Average(a => a.ResultCount) : 0 },
                { "averageSearchDuration", analytics.Any() ? analytics.Where(a => a.SearchDurationMs.HasValue).Average(a => a.SearchDurationMs.Value) : 0 },
                { "averageQualityScore", analytics.Any() ? analytics.Average(a => a.QualityScore) : 0 },
                { "clickThroughRate", analytics.Any() ? (double)analytics.Count(a => a.UserClickedResult) / analytics.Count : 0 },
                { "zeroResultSearches", analytics.Count(a => a.ResultCount == 0) }
            };
        }
    }
}
