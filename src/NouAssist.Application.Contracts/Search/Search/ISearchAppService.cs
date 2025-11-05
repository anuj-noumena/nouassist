using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Search.Search.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Search.Search
{
    public interface ISearchAppService : IApplicationService
    {
        Task<SearchResultsDto> SearchAsync(SearchRequestDto input);
        Task<List<SearchSuggestionDto>> GetSuggestionsAsync(string partialQuery);
        Task<List<SavedSearchDto>> GetSavedSearchesAsync();
        Task<SavedSearchDto> SaveSearchAsync(CreateSavedSearchDto input);
        Task DeleteSavedSearchAsync(Guid id);
        Task<SavedSearchDto> ExecuteSavedSearchAsync(Guid id);
        Task<SearchAnalyticsDto> GetMyAnalyticsAsync(DateTime startDate, DateTime endDate);
        Task ClearSearchHistoryAsync();
    }
}
