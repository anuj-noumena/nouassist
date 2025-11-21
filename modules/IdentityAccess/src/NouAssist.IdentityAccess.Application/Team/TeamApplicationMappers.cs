using NouAssist.IdentityAccess.Team.Aggregates;
using NouAssist.IdentityAccess.Team.Entities;
using NouAssist.IdentityAccess.Team.Roles.Dtos;
using NouAssist.IdentityAccess.Team.Teams.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.IdentityAccess.Team
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TeamToTeamDtoMapper : MapperBase<Aggregates.Team, TeamDto>
    {
        [MapperIgnoreTarget(nameof(TeamDto.ActiveMemberCount))]
        [MapperIgnoreTarget(nameof(TeamDto.ParentTeamName))]
        [MapperIgnoreTarget(nameof(TeamDto.LeaderName))]
        public override partial TeamDto Map(Aggregates.Team source);

        public override partial void Map(Aggregates.Team source, TeamDto destination);

        public override void AfterMap(Aggregates.Team source, TeamDto destination)
        {
            destination.ActiveMemberCount = source.GetActiveMemberCount();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TeamMemberToTeamMemberDtoMapper : MapperBase<TeamMember, TeamMemberDto>
    {
        [MapperIgnoreTarget(nameof(TeamMemberDto.UserName))]
        [MapperIgnoreTarget(nameof(TeamMemberDto.Email))]
        public override partial TeamMemberDto Map(TeamMember source);

        public override partial void Map(TeamMember source, TeamMemberDto destination);

        public override void AfterMap(TeamMember source, TeamMemberDto destination)
        {
            destination.IsActive = source.IsActive;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RoleToRoleDtoMapper : MapperBase<Role, RoleDto>
    {
        [MapperIgnoreTarget(nameof(RoleDto.CurrentUserCount))]
        public override partial RoleDto Map(Role source);

        public override partial void Map(Role source, RoleDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class UserRoleToUserRoleDtoMapper : MapperBase<UserRole, UserRoleDto>
    {
        [MapperIgnoreTarget(nameof(UserRoleDto.UserName))]
        [MapperIgnoreTarget(nameof(UserRoleDto.RoleName))]
        public override partial UserRoleDto Map(UserRole source);

        public override partial void Map(UserRole source, UserRoleDto destination);

        public override void AfterMap(UserRole source, UserRoleDto destination)
        {
            destination.IsExpired = source.IsExpired;
        }
    }
}
