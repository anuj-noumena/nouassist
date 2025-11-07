using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Infrastructure.SearchEngine.Aggregates;
using NouAssist.Infrastructure.SearchEngine.Data;
using NouAssist.Infrastructure.SearchEngine.Enums;
using NouAssist.Infrastructure.SearchEngine.Permissions;
using NouAssist.Infrastructure.SearchEngine.Search.Dtos;
using NouAssist.Infrastructure.SearchEngine.Services;
using NouAssist.Infrastructure.SearchEngine.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure.SearchEngine.Search
{
    [Authorize(SearchPermissions.Search.Default)]
    public class SearchAppService : ApplicationService, ISearchAppService
    {
        private readonly ISearchHistoryRepository _searchHistoryRepository;
        private readonly ISavedSearchRepository _savedSearchRepository;
        private readonly IPopularSearchRepository _popularSearchRepository;
        private readonly SearchSuggestionService _suggestionService;
        private readonly SearchAnalyticsService _analyticsService;

        public SearchAppService(
            ISearchHistoryRepository searchHistoryRepository,
            ISavedSearchRepository savedSearchRepository,
            IPopularSearchRepository popularSearchRepository,
            SearchSuggestionService suggestionService,
            SearchAnalyticsService analyticsService)
        {
            _searchHistoryRepository = searchHistoryRepository;
            _savedSearchRepository = savedSearchRepository;
            _popularSearchRepository = popularSearchRepository;
            _suggestionService = suggestionService;
            _analyticsService = analyticsService;
        }

        public async Task<SearchResultsDto> SearchAsync(SearchRequestDto input)
        {
            var stopwatch = Stopwatch.StartNew();

            // TODO: Implement actual search logic against your search engine (Elasticsearch, etc.)
            // For now, return mock results
            var mockResults = CreateMockSearchResults(input);

            stopwatch.Stop();

            // Record search analytics
            await _analyticsService.RecordSearchAsync(
                CurrentUser.Id.Value,
                input.Query,
                mockResults,
                (int)stopwatch.ElapsedMilliseconds,
                CurrentTenant.Id,
                input.EntityTypes,
                input.Filters
            );

            var dto = ObjectMapper.Map<SearchResults, SearchResultsDto>(mockResults);
            dto.SearchDurationMs = (int)stopwatch.ElapsedMilliseconds;

            return dto;
        }

        public async Task<List<SearchSuggestionDto>> GetSuggestionsAsync(string partialQuery)
        {
            if (string.IsNullOrWhiteSpace(partialQuery) || partialQuery.Length < 2)
            {
                return new List<SearchSuggestionDto>();
            }

            var suggestions = await _suggestionService.GetSuggestionsAsync(
                CurrentUser.Id.Value,
                partialQuery,
                10
            );

            return ObjectMapper.Map<List<SearchSuggestion>, List<SearchSuggestionDto>>(suggestions);
        }

        [Authorize(SearchPermissions.Search.SaveSearches)]
        public async Task<List<SavedSearchDto>> GetSavedSearchesAsync()
        {
            var savedSearches = await _savedSearchRepository.GetByUserAsync(CurrentUser.Id.Value);
            return ObjectMapper.Map<List<SavedSearch>, List<SavedSearchDto>>(savedSearches);
        }

        [Authorize(SearchPermissions.Search.SaveSearches)]
        public async Task<SavedSearchDto> SaveSearchAsync(CreateSavedSearchDto input)
        {
            // Check if already exists
            var existing = await _savedSearchRepository.FindByNameAsync(CurrentUser.Id.Value, input.Name);
            
            if (existing != null)
            {
                throw new BusinessException("Search:DuplicateName", "A saved search with this name already exists.");
            }

            var savedSearch = new SavedSearch(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                input.Name,
                input.Query,
                CurrentTenant.Id,
                input.Filters
            );

            await _savedSearchRepository.InsertAsync(savedSearch);

            return ObjectMapper.Map<SavedSearch, SavedSearchDto>(savedSearch);
        }

        [Authorize(SearchPermissions.Search.SaveSearches)]
        public async Task DeleteSavedSearchAsync(Guid id)
        {
            var savedSearch = await _savedSearchRepository.GetAsync(id);

            // Authorization check
            if (savedSearch.UserId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Search:NotAuthorized", "You are not authorized to delete this saved search.");
            }

            await _savedSearchRepository.DeleteAsync(savedSearch);
        }

        [Authorize(SearchPermissions.Search.SaveSearches)]
        public async Task<SavedSearchDto> ExecuteSavedSearchAsync(Guid id)
        {
            var savedSearch = await _savedSearchRepository.GetAsync(id);

            // Authorization check
            if (savedSearch.UserId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Search:NotAuthorized", "You are not authorized to execute this saved search.");
            }

            // Update usage statistics
            savedSearch.IncrementUsage();
            await _savedSearchRepository.UpdateAsync(savedSearch);

            // Execute the search (you would then return the search results)
            // For now, just return the saved search DTO
            return ObjectMapper.Map<SavedSearch, SavedSearchDto>(savedSearch);
        }

        [Authorize(SearchPermissions.Search.ViewAnalytics)]
        public async Task<SearchAnalyticsDto> GetMyAnalyticsAsync(DateTime startDate, DateTime endDate)
        {
            var analytics = await _analyticsService.GetUserAnalyticsAsync(
                CurrentUser.Id.Value,
                startDate,
                endDate
            );

            // Get popular searches for this user
            var popularSearches = await _popularSearchRepository.GetTopSearchesAsync(
                CurrentUser.Id.Value,
                startDate,
                endDate,
                10
            );

            var topQueries = popularSearches.Select(p => new PopularQueryDto
            {
                Query = p.Query,
                SearchCount = p.SearchCount
            }).ToList();

            return new SearchAnalyticsDto
            {
                TotalSearches = analytics.TotalSearches,
                AverageResultCount = analytics.AverageResultCount,
                AverageSearchDuration = analytics.AverageSearchDuration,
                AverageQualityScore = analytics.AverageQualityScore,
                ClickThroughRate = analytics.ClickThroughRate,
                ZeroResultSearches = analytics.ZeroResultSearches,
                TopQueries = topQueries
            };
        }

        [Authorize(SearchPermissions.Search.ViewHistory)]
        public async Task ClearSearchHistoryAsync()
        {
            var history = await _searchHistoryRepository.GetByUserAsync(CurrentUser.Id.Value);
            
            foreach (var item in history)
            {
                await _searchHistoryRepository.DeleteAsync(item);
            }
        }

        // Helper method to create mock search results
        private SearchResults CreateMockSearchResults(SearchRequestDto input)
        {
            // In production, this would call your actual search engine
            var items = new List<SearchResultItem>();

            // Create some mock items for demonstration
            for (int i = 0; i < Math.Min(input.Limit, 10); i++)
            {
                items.Add(new SearchResultItem
                {
                    Id = Guid.NewGuid(),
                    EntityType = SearchableEntityType.Document,
                    Title = $"Result {i + 1} for '{input.Query}'",
                    Description = $"This is a mock search result matching your query: {input.Query}",
                    Snippet = $"...{input.Query}...",
                    Metadata = new Dictionary<string, object>
                    {
                        { "score", 0.95 - (i * 0.05) }
                    },
                    RelevanceScore = 0.95 - (i * 0.05),
                    CreatedAt = DateTime.UtcNow.AddDays(-i),
                    Tags = new List<string> { "tag1", "tag2" }
                });
            }

            return new SearchResults
            {
                Query = input.Query,
                Items = items,
                TotalCount = items.Count > 0 ? 100 : 0, // Mock total
                Offset = input.Offset,
                Limit = input.Limit,
                Facets = new Dictionary<string, int>
                {
                    { "Document", 45 },
                    { "Book", 30 },
                    { "Note", 25 }
                },
                Suggestions = new List<string>(),
                HasResults = items.Any()
            };
        }
    }
}
