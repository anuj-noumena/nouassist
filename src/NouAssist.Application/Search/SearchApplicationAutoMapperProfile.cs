using AutoMapper;
using NouAssist.Search.Aggregates;
using NouAssist.Search.Search.Dtos;
using NouAssist.Search.ValueObjects;

namespace NouAssist.Search
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
