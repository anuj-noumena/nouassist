using AutoMapper;
using NouAssist.IdentityAccess.Users.Aggregates;
using NouAssist.IdentityAccess.Users.Users.Dtos;

namespace NouAssist.IdentityAccess.Users
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
