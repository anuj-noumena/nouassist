using AutoMapper;
using NouAssist.Infrastructure.SearchEngine.Aggregates;
using NouAssist.Infrastructure.SearchEngine.Search.Dtos;
using NouAssist.Infrastructure.SearchEngine.ValueObjects;

namespace NouAssist.Infrastructure.SearchEngine
{
    public class SearchApplicationAutoMapperProfile : Profile
    {
        public SearchApplicationAutoMapperProfile()
        {
            CreateMap<SavedSearch, SavedSearchDto>();
            CreateMap<SearchSuggestion, SearchSuggestionDto>()
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source.ToString()));
            CreateMap<SearchResultItem, SearchResultItemDto>();
            CreateMap<SearchResults, SearchResultsDto>();
        }
    }
}
