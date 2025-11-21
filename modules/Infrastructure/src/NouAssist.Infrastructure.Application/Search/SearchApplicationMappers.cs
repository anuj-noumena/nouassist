using NouAssist.Infrastructure.Search.Aggregates;
using NouAssist.Infrastructure.Search.Search.Dtos;
using NouAssist.Infrastructure.Search.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.Infrastructure.Search
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SavedSearchToSavedSearchDtoMapper : MapperBase<SavedSearch, SavedSearchDto>
    {
        public override partial SavedSearchDto Map(SavedSearch source);

        public override partial void Map(SavedSearch source, SavedSearchDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SearchSuggestionToSearchSuggestionDtoMapper : MapperBase<SearchSuggestion, SearchSuggestionDto>
    {
        [MapperIgnoreTarget(nameof(SearchSuggestionDto.Source))]
        public override partial SearchSuggestionDto Map(SearchSuggestion source);

        [MapperIgnoreTarget(nameof(SearchSuggestionDto.Source))]
        public override partial void Map(SearchSuggestion source, SearchSuggestionDto destination);

        public override void AfterMap(SearchSuggestion source, SearchSuggestionDto destination)
        {
            destination.Source = source.Source.ToString();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SearchResultItemToSearchResultItemDtoMapper : MapperBase<SearchResultItem, SearchResultItemDto>
    {
        public override partial SearchResultItemDto Map(SearchResultItem source);

        public override partial void Map(SearchResultItem source, SearchResultItemDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SearchResultsToSearchResultsDtoMapper : MapperBase<SearchResults, SearchResultsDto>
    {
        [MapperIgnoreTarget(nameof(SearchResultsDto.SearchDurationMs))]
        public override partial SearchResultsDto Map(SearchResults source);

        [MapperIgnoreTarget(nameof(SearchResultsDto.SearchDurationMs))]
        public override partial void Map(SearchResults source, SearchResultsDto destination);
    }
}
