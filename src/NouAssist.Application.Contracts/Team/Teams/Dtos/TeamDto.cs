using System;
using System.Collections.Generic;
using NouAssist.Team.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Team.Teams.Dtos
{
    public class TeamDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Color { get; set; }
        public TeamType TeamType { get; set; }
        public Guid? ParentTeamId { get; set; }
        public string ParentTeamName { get; set; }
        public Guid? LeaderId { get; set; }
        public string LeaderName { get; set; }
        public List<Guid> AdminIds { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrivate { get; set; }
        public int? MaxMembers { get; set; }
        public int ActiveMemberCount { get; set; }
        public List<TeamMemberDto> Members { get; set; }
    }

    public class TeamMemberDto : FullAuditedEntityDto<Guid>
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public TeamMemberRole Role { get; set; }
        public TeamMemberStatus Status { get; set; }
        public string Title { get; set; }
        public DateTime? JoinedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTeamDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TeamType TeamType { get; set; }
        public Guid? ParentTeamId { get; set; }
        public Guid? LeaderId { get; set; }
        public bool IsPrivate { get; set; }
    }

    public class UpdateTeamDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Color { get; set; }
        public Guid? LeaderId { get; set; }
        public int? MaxMembers { get; set; }
        public bool IsPrivate { get; set; }
    }

    public class AddTeamMemberDto
    {
        public Guid UserId { get; set; }
        public TeamMemberRole Role { get; set; }
        public string Title { get; set; }
    }

    public class UpdateTeamMemberRoleDto
    {
        public TeamMemberRole NewRole { get; set; }
    }

    public class GetTeamListDto : PagedAndSortedResultRequestDto
    {
        public TeamType? TeamType { get; set; }
        public bool? IsActive { get; set; }
        public Guid? LeaderId { get; set; }
        public string Filter { get; set; }
    }
}
