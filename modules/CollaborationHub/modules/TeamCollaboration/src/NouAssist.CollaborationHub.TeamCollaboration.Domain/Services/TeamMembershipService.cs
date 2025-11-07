using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.CollaborationHub.TeamCollaboration.Aggregates;
using NouAssist.CollaborationHub.TeamCollaboration.Entities;
using NouAssist.CollaborationHub.TeamCollaboration.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Microsoft.Extensions.Logging;

namespace NouAssist.CollaborationHub.TeamCollaboration.Services
{
    public class TeamMembershipService : DomainService
    {
        public TeamMembershipService()
        {
        }

        public TeamMembershipResult AddMemberToTeam(
            Aggregates.Team team,
            Guid userId,
            TeamMemberRole role,
            Guid invitedBy,
            string title = null
        )
        {
            var result = new TeamMembershipResult();

            // Validate team is active
            if (!team.IsActive)
            {
                result.AddError("Team is not active");
                return result;
            }

            // Validate inviter has permission
            if (!team.IsAdmin(invitedBy) && !team.IsLeader(invitedBy))
            {
                result.AddError("Only team admins and leaders can invite members");
                return result;
            }

            // Check if user is already a member
            if (team.IsMember(userId))
            {
                result.AddError("User is already a member of this team");
                return result;
            }

            try
            {
                var member = team.AddMember(userId, role, invitedBy, title);
                result.Member = member;
                result.IsSuccessful = true;
                result.Message = "Member added successfully";
            }
            catch (BusinessException ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }

        public TeamMembershipResult RemoveMemberFromTeam(
            Aggregates.Team team,
            Guid userId,
            Guid removedBy,
            string reason
        )
        {
            var result = new TeamMembershipResult();

            // Validate remover has permission
            if (!team.IsAdmin(removedBy) && !team.IsLeader(removedBy))
            {
                result.AddError("Only team admins and leaders can remove members");
                return result;
            }

            // Cannot remove yourself if you're the last admin
            if (userId == removedBy && team.IsAdmin(userId))
            {
                var otherAdmins = team.Members.Count(m =>
                    m.IsActive &&
                    m.UserId != userId &&
                    team.IsAdmin(m.UserId));

                if (otherAdmins == 0)
                {
                    result.AddError("Cannot remove yourself as the last admin");
                    return result;
                }
            }

            try
            {
                team.RemoveMember(userId, reason);
                result.IsSuccessful = true;
                result.Message = "Member removed successfully";
            }
            catch (BusinessException ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }

        public TeamMembershipResult PromoteMember(
            Aggregates.Team team,
            Guid userId,
            TeamMemberRole newRole,
            Guid promotedBy
        )
        {
            var result = new TeamMembershipResult();

            // Validate promoter has permission
            if (!team.IsLeader(promotedBy))
            {
                result.AddError("Only team leaders can promote members");
                return result;
            }

            // Cannot promote to owner
            if (newRole == TeamMemberRole.Owner)
            {
                result.AddError("Cannot promote to owner role");
                return result;
            }

            try
            {
                team.UpdateMemberRole(userId, newRole);

                // Add to admin list if promoting to admin
                if (newRole == TeamMemberRole.Admin)
                {
                    team.AddAdmin(userId);
                }

                result.IsSuccessful = true;
                result.Message = $"Member promoted to {newRole}";
            }
            catch (BusinessException ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }

        public List<Guid> GetUserTeams(List<TeamMember> memberships)
        {
            return memberships
                .Where(m => m.IsActive)
                .Select(m => m.TeamId)
                .Distinct()
                .ToList();
        }

        public bool CanUserAccessTeam(Aggregates.Team team, Guid userId, List<TeamMember> memberships)
        {
            // Public teams are accessible to all
            if (!team.IsPrivate)
            {
                return true;
            }

            // Private teams require membership
            return memberships.Any(m =>
                m.TeamId == team.Id &&
                m.UserId == userId &&
                m.IsActive);
        }
    }

    public class TeamMembershipResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public TeamMember Member { get; set; }
        public List<string> Errors { get; private set; } = new();

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public bool HasErrors => Errors.Any();
    }
}
