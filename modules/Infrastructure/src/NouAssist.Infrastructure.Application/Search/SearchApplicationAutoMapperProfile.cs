using AutoMapper;
using NouAssist.Infrastructure.Search.Aggregates;
using NouAssist.Infrastructure.Search.Search.Dtos;
using NouAssist.Infrastructure.Search.ValueObjects;

namespace NouAssist.Infrastructure.Search
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
