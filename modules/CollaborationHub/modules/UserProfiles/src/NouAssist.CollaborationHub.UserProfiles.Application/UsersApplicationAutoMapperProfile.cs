using AutoMapper;
using NouAssist.CollaborationHub.UserProfiles.Aggregates;
using NouAssist.CollaborationHub.UserProfiles.Dtos;

namespace NouAssist.CollaborationHub.UserProfiles
{
    public class UsersApplicationAutoMapperProfile : Profile
    {
        public UsersApplicationAutoMapperProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.GetFullName()))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.GetDisplayName()))
                .ForMember(dest => dest.Initials, opt => opt.MapFrom(src => src.GetInitials()));

            CreateMap<UserSettings, UserSettingsDto>();
            CreateMap<UserEisenhowerPreference, UserEisenhowerPreferenceDto>();
        }
    }
}
