using AutoMapper;
using NouAssist.Core.Aggregates;
using NouAssist.Core.Dtos;

namespace NouAssist.Core
{
    public class CoreApplicationAutoMapperProfile : Profile
    {
        public CoreApplicationAutoMapperProfile()
        {
            CreateMap<Tag, TagDto>()
                .ForMember(dest => dest.IsPopular, opt => opt.MapFrom(src => src.IsPopular));

            CreateMap<TagCategory, TagCategoryDto>();

            CreateMap<Context, ContextDto>()
                .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(src => src.IsArchived));

            CreateMap<Area, AreaDto>()
                .ForMember(dest => dest.HasParent, opt => opt.MapFrom(src => src.HasParent))
                .ForMember(dest => dest.IsRootArea, opt => opt.MapFrom(src => src.IsRootArea))
                .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(src => src.IsArchived));

            CreateMap<Area, AreaWithChildrenDto>()
                .IncludeBase<Area, AreaDto>();

            CreateMap<InboxItem, InboxItemDto>()
                .ForMember(dest => dest.AgeInDays, opt => opt.MapFrom(src => src.AgeInDays))
                .ForMember(dest => dest.IsStale, opt => opt.MapFrom(src => src.IsStale));

            CreateMap<LogActivity, LogActivityDto>();
        }
    }
}
