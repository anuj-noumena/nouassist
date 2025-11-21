using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.IdentityAccess.Team.Data;
using NouAssist.IdentityAccess.Team.Enums;
using NouAssist.IdentityAccess.Permissions;
using NouAssist.IdentityAccess.Team.Services;
using NouAssist.IdentityAccess.Team.Specifications;
using NouAssist.SharedKernel.Specifications;
using NouAssist.IdentityAccess.Team.Teams.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.IdentityAccess.Team.Teams
{
    [Authorize(IdentityAccessPermissions.Team.Teams.Default)]
    public class TeamAppService : ApplicationService, ITeamAppService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly TeamMembershipService _membershipService;

        public TeamAppService(
            ITeamRepository teamRepository,
            TeamMembershipService membershipService)
        {
            _teamRepository = teamRepository;
            _membershipService = membershipService;
        }

        public async Task<TeamDto> GetAsync(Guid id)
        {
            var team = await _teamRepository.GetWithMembersAsync(id);
            return await MapToDto(team);
        }

        public async Task<PagedResultDto<TeamDto>> GetListAsync(GetTeamListDto input)
        {
            var spec = CreateListSpecification(input);

            var teams = await _teamRepository.GetListAsync(spec);
            var totalCount = await _teamRepository.LongCountAsync(spec.ToExpression());

            var dtos = new List<TeamDto>();
            foreach (var team in teams)
            {
                dtos.Add(await MapToDto(team));
            }

            return new PagedResultDto<TeamDto>(totalCount, dtos);
        }

        public async Task<List<TeamDto>> GetMyTeamsAsync()
        {
            var teams = await _teamRepository.GetUserTeamsAsync(CurrentUser.Id.Value);
            
            var dtos = new List<TeamDto>();
            foreach (var team in teams)
            {
                dtos.Add(await MapToDto(team));
            }

            return dtos;
        }

        public async Task<List<TeamDto>> GetUserTeamsAsync(Guid userId)
        {
            var teams = await _teamRepository.GetUserTeamsAsync(userId);
            
            var dtos = new List<TeamDto>();
            foreach (var team in teams)
            {
                dtos.Add(await MapToDto(team));
            }

            return dtos;
        }

        [Authorize(IdentityAccessPermissions.Team.Teams.Create)]
        public async Task<TeamDto> CreateAsync(CreateTeamDto input)
        {
            var team = new Team.Aggregates.Team(
                GuidGenerator.Create(),
                input.Name,
                input.TeamType,
                CurrentTenant.Id,
                input.Description
            );

            if (input.ParentTeamId.HasValue)
            {
                team.SetParentTeam(input.ParentTeamId.Value);
            }

            if (input.IsPrivate)
            {
                team.MakePrivate();
            }

            // Add creator as owner
            team.AddMember(
                CurrentUser.Id.Value,
                TeamMemberRole.Owner,
                CurrentUser.Id.Value
            );

            if (input.LeaderId.HasValue)
            {
                team.AssignLeader(input.LeaderId.Value);
            }

            await _teamRepository.InsertAsync(team);

            return await MapToDto(team);
        }

        [Authorize(IdentityAccessPermissions.Team.Teams.Update)]
        public async Task<TeamDto> UpdateAsync(Guid id, UpdateTeamDto input)
        {
            var team = await _teamRepository.GetAsync(id);

            // Check permission
            if (!team.IsAdmin(CurrentUser.Id.Value) && !team.IsLeader(CurrentUser.Id.Value))
            {
                throw new UserFriendlyException("You don't have permission to update this team");
            }

            team.SetName(input.Name);
            team.SetDescription(input.Description);

            if (!string.IsNullOrEmpty(input.Avatar))
            {
                team.SetAvatar(input.Avatar);
            }

            if (!string.IsNullOrEmpty(input.Color))
            {
                team.SetColor(input.Color);
            }

            if (input.LeaderId.HasValue)
            {
                team.AssignLeader(input.LeaderId.Value);
            }

            if (input.MaxMembers.HasValue)
            {
                team.SetMaxMembers(input.MaxMembers.Value);
            }

            if (input.IsPrivate)
            {
                team.MakePrivate();
            }
            else
            {
                team.MakePublic();
            }

            await _teamRepository.UpdateAsync(team);

            return await MapToDto(team);
        }

        [Authorize(IdentityAccessPermissions.Team.Teams.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var team = await _teamRepository.GetAsync(id);

            if (!team.IsAdmin(CurrentUser.Id.Value) && !team.IsLeader(CurrentUser.Id.Value))
            {
                throw new UserFriendlyException("You don't have permission to delete this team");
            }

            await _teamRepository.DeleteAsync(id);
        }

        [Authorize(IdentityAccessPermissions.Team.Teams.Update)]
        public async Task<TeamDto> ActivateAsync(Guid id)
        {
            var team = await _teamRepository.GetAsync(id);
            team.Activate();
            await _teamRepository.UpdateAsync(team);

            return await MapToDto(team);
        }

        [Authorize(IdentityAccessPermissions.Team.Teams.Update)]
        public async Task<TeamDto> DeactivateAsync(Guid id)
        {
            var team = await _teamRepository.GetAsync(id);
            team.Deactivate();
            await _teamRepository.UpdateAsync(team);

            return await MapToDto(team);
        }

        [Authorize(IdentityAccessPermissions.Team.Teams.ManageMembers)]
        public async Task<TeamMemberDto> AddMemberAsync(Guid teamId, AddTeamMemberDto input)
        {
            var team = await _teamRepository.GetWithMembersAsync(teamId);

            var result = _membershipService.AddMemberToTeam(
                team,
                input.UserId,
                input.Role,
                CurrentUser.Id.Value,
                input.Title
            );

            if (!result.IsSuccessful)
            {
                throw new UserFriendlyException(string.Join(", ", result.Errors));
            }

            await _teamRepository.UpdateAsync(team);

            var dto = ObjectMapper.Map<Entities.TeamMember, TeamMemberDto>(result.Member);
            dto.UserName = $"User-{input.UserId}"; // Load from identity service in real scenario
            return dto;
        }

        [Authorize(IdentityAccessPermissions.Team.Teams.ManageMembers)]
        public async Task RemoveMemberAsync(Guid teamId, Guid userId, string reason)
        {
            var team = await _teamRepository.GetWithMembersAsync(teamId);

            var result = _membershipService.RemoveMemberFromTeam(
                team,
                userId,
                CurrentUser.Id.Value,
                reason
            );

            if (!result.IsSuccessful)
            {
                throw new UserFriendlyException(string.Join(", ", result.Errors));
            }

            await _teamRepository.UpdateAsync(team);
        }

        [Authorize(IdentityAccessPermissions.Team.Members.UpdateRole)]
        public async Task<TeamMemberDto> UpdateMemberRoleAsync(
            Guid teamId, 
            Guid userId, 
            UpdateTeamMemberRoleDto input)
        {
            var team = await _teamRepository.GetWithMembersAsync(teamId);

            var result = _membershipService.PromoteMember(
                team,
                userId,
                input.NewRole,
                CurrentUser.Id.Value
            );

            if (!result.IsSuccessful)
            {
                throw new UserFriendlyException(string.Join(", ", result.Errors));
            }

            await _teamRepository.UpdateAsync(team);

            var member = team.Members.First(m => m.UserId == userId);
            var dto = ObjectMapper.Map<Entities.TeamMember, TeamMemberDto>(member);
            dto.UserName = $"User-{userId}";
            return dto;
        }

        public async Task<TeamMemberDto> AcceptInvitationAsync(Guid teamId)
        {
            var team = await _teamRepository.GetWithMembersAsync(teamId);

            team.AcceptMemberInvitation(CurrentUser.Id.Value);

            await _teamRepository.UpdateAsync(team);

            var member = team.Members.First(m => m.UserId == CurrentUser.Id.Value);
            var dto = ObjectMapper.Map<Entities.TeamMember, TeamMemberDto>(member);
            dto.UserName = CurrentUser.UserName;
            return dto;
        }

        public async Task<List<TeamMemberDto>> GetTeamMembersAsync(Guid teamId)
        {
            var team = await _teamRepository.GetWithMembersAsync(teamId);

            var dtos = ObjectMapper.Map<List<Entities.TeamMember>, List<TeamMemberDto>>(
                team.Members.ToList());

            foreach (var dto in dtos)
            {
                dto.UserName = $"User-{dto.UserId}"; // Load from identity service in real scenario
            }

            return dtos;
        }

        private ISpecification<Team.Aggregates.Team> CreateListSpecification(GetTeamListDto input)
        {
            ISpecification<Team.Aggregates.Team> spec = new BaseSpecification<Team.Aggregates.Team>();

            if (input.TeamType.HasValue)
            {
                spec = spec.And(new TeamsByTypeSpecification(input.TeamType.Value));
            }

            if (input.IsActive.HasValue)
            {
                if (input.IsActive.Value)
                {
                    spec = spec.And(new ActiveTeamsSpecification());
                }
                else
                {
                    spec = spec.And(new BaseSpecification<Team.Aggregates.Team>(t => !t.IsActive));
                }
            }

            if (input.LeaderId.HasValue)
            {
                spec = spec.And(new TeamsByLeaderSpecification(input.LeaderId.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                spec = spec.And(new BaseSpecification<Team.Aggregates.Team>(
                    t => t.Name.Contains(input.Filter) ||
                         (t.Description != null && t.Description.Contains(input.Filter))));
            }

            return spec;
        }

        private async Task<TeamDto> MapToDto(Team.Aggregates.Team team)
        {
            var dto = ObjectMapper.Map<Team.Aggregates.Team, TeamDto>(team);

            // Load parent team name
            if (team.ParentTeamId.HasValue)
            {
                var parentTeam = await _teamRepository.FindAsync(team.ParentTeamId.Value);
                if (parentTeam != null)
                {
                    dto.ParentTeamName = parentTeam.Name;
                }
            }

            // Load leader name
            if (team.LeaderId.HasValue)
            {
                dto.LeaderName = $"User-{team.LeaderId.Value}"; // Load from identity service
            }

            return dto;
        }
    }
}
