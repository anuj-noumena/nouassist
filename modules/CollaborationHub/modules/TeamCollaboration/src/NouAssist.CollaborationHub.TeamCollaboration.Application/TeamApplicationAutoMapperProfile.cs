using AutoMapper;
using NouAssist.CollaborationHub.TeamCollaboration.Aggregates;
using NouAssist.CollaborationHub.TeamCollaboration.Entities;
using NouAssist.CollaborationHub.TeamCollaboration.Roles.Dtos;
using NouAssist.CollaborationHub.TeamCollaboration.Teams.Dtos;

namespace NouAssist.CollaborationHub.TeamCollaboration
{
    public class TeamApplicationAutoMapperProfile : Profile
    {
        public TeamApplicationAutoMapperProfile()
        {
            CreateMap<Team.Aggregates.Team, TeamDto>()
                .ForMember(dest => dest.ActiveMemberCount, 
                    opt => opt.MapFrom(src => src.GetActiveMemberCount()))
                .ForMember(dest => dest.ParentTeamName, opt => opt.Ignore())
                .ForMember(dest => dest.LeaderName, opt => opt.Ignore());

            CreateMap<TeamMember, TeamMemberDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore());

            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.CurrentUserCount, opt => opt.Ignore());

            CreateMap<UserRole, UserRoleDto>()
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired))
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.RoleName, opt => opt.Ignore());
        }
    }
}
