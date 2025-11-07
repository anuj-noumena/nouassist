using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.CollaborationHub.TeamCollaboration.Teams.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.TeamCollaboration.Teams
{
    public interface ITeamAppService : IApplicationService
    {
        Task<TeamDto> GetAsync(Guid id);
        Task<PagedResultDto<TeamDto>> GetListAsync(GetTeamListDto input);
        Task<List<TeamDto>> GetMyTeamsAsync();
        Task<List<TeamDto>> GetUserTeamsAsync(Guid userId);
        Task<TeamDto> CreateAsync(CreateTeamDto input);
        Task<TeamDto> UpdateAsync(Guid id, UpdateTeamDto input);
        Task DeleteAsync(Guid id);
        Task<TeamDto> ActivateAsync(Guid id);
        Task<TeamDto> DeactivateAsync(Guid id);
        Task<TeamMemberDto> AddMemberAsync(Guid teamId, AddTeamMemberDto input);
        Task RemoveMemberAsync(Guid teamId, Guid userId, string reason);
        Task<TeamMemberDto> UpdateMemberRoleAsync(Guid teamId, Guid userId, UpdateTeamMemberRoleDto input);
        Task<TeamMemberDto> AcceptInvitationAsync(Guid teamId);
        Task<List<TeamMemberDto>> GetTeamMembersAsync(Guid teamId);
    }
}
