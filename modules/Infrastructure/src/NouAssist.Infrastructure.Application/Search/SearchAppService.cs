using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Infrastructure.Permissions;
using NouAssist.Infrastructure.Search.Aggregates;
using NouAssist.Infrastructure.Search.Data;
using NouAssist.Infrastructure.Search.Enums;
using NouAssist.Infrastructure.Search.Search.Dtos;
using NouAssist.Infrastructure.Search.Services;
using NouAssist.Infrastructure.Search.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure.Search
{
    [Authorize(InfrastructurePermissions.Search.SearchFeatures.Default)]
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

        [Authorize(InfrastructurePermissions.Search.SearchFeatures.SaveSearches)]
        public async Task<List<SavedSearchDto>> GetSavedSearchesAsync()
        {
            var savedSearches = await _savedSearchRepository.GetByUserAsync(CurrentUser.Id.Value);
            return ObjectMapper.Map<List<SavedSearch>, List<SavedSearchDto>>(savedSearches);
        }

        [Authorize(InfrastructurePermissions.Search.SearchFeatures.SaveSearches)]
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

        [Authorize(InfrastructurePermissions.Search.SearchFeatures.SaveSearches)]
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

        [Authorize(InfrastructurePermissions.Search.SearchFeatures.SaveSearches)]
        public async Task<SavedSearchDto> ExecuteSavedSearchAsync(Guid id)
        {
            var savedSearch = await _savedSearchRepository.GetAsync(id);

            // Authorization check
            if (savedSearch.UserId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Search:NotAuthorized", "You are not authorized to execute this saved search.");
            }

            // Update usage statistics
            savedSearch.RecordUsage();
            await _savedSearchRepository.UpdateAsync(savedSearch);

            // Execute the search (you would then return the search results)
            // For now, just return the saved search DTO
            return ObjectMapper.Map<SavedSearch, SavedSearchDto>(savedSearch);
        }

        [Authorize(InfrastructurePermissions.Search.SearchFeatures.ViewAnalytics)]
        public async Task<SearchAnalyticsDto> GetMyAnalyticsAsync(DateTime startDate, DateTime endDate)
        {
            var analytics = await _analyticsService.GetSearchMetricsAsync(
                CurrentUser.Id.Value,
                startDate,
                endDate
            );

            // Get popular searches for this user
            var popularSearches = await _popularSearchRepository.GetTopSearchesAsync(10);

            var topQueries = popularSearches
                .Where(p => p.CreationTime >= startDate && p.CreationTime <= endDate)
                .Select(p => new PopularQueryDto
                {
                    Query = p.Query,
                    SearchCount = p.SearchCount
                }).ToList();

            return new SearchAnalyticsDto
            {
                TotalSearches = (int)analytics["totalSearches"],
                AverageResultCount = (double)analytics["averageResultCount"],
                AverageSearchDuration = (double)analytics["averageSearchDuration"],
                AverageQualityScore = (double)analytics["averageQualityScore"],
                ClickThroughRate = (double)analytics["clickThroughRate"],
                ZeroResultSearches = (int)analytics["zeroResultSearches"],
                TopQueries = topQueries
            };
        }

        [Authorize(InfrastructurePermissions.Search.SearchFeatures.ViewHistory)]
        public async Task ClearSearchHistoryAsync()
        {
            var history = await _searchHistoryRepository.GetRecentByUserAsync(CurrentUser.Id.Value);
            
            foreach (var item in history)
            {
                await _searchHistoryRepository.DeleteAsync(item.Id);
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
                items.Add(new SearchResultItem(
                    Guid.NewGuid(),
                    SearchableEntityType.Resource.ToString(),
                    $"Result {i + 1} for '{input.Query}'",
                    $"This is a mock search result matching your query: {input.Query}",
                    $"...{input.Query}...",
                    new Dictionary<string, object> { { "score", 0.95 - (i * 0.05) } },
                    0.95 - (i * 0.05),
                    DateTime.UtcNow.AddDays(-i)
                ));
            }

            return new SearchResults(
                input.Query,
                items,
                items.Count > 0 ? 100 : 0, // Mock total
                input.Offset,
                input.Limit,
                TimeSpan.FromMilliseconds(100), // Mock duration
                new Dictionary<string, int>
                {
                    { "Document", 45 },
                    { "Book", 30 },
                    { "Note", 25 }
                },
                new List<string>()
            );
        }
    }
}
