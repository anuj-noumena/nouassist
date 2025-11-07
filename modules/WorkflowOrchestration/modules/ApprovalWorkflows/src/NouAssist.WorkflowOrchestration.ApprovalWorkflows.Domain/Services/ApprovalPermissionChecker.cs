using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Aggregates;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Entities;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.Services
{
    public class ApprovalPermissionChecker : DomainService
    {
        public bool CanUserApproveStep(
            Guid userId,
            ApprovalStep step,
            List<Guid> userRoleIds,
            List<Guid> userTeamIds,
            Guid? requestedBy = null
        )
        {
            // Check if self-approval is allowed
            if (requestedBy.HasValue && 
                requestedBy.Value == userId && 
                !step.AllowSelfApproval)
            {
                return false;
            }

            // Check direct user assignment
            if (step.ApproverUserIds.Contains(userId))
            {
                return true;
            }

            // Check role-based assignment
            if (step.ApproverRoleIds.Any(roleId => userRoleIds.Contains(roleId)))
            {
                return true;
            }

            // Check team-based assignment
            if (step.ApproverTeamIds.Any(teamId => userTeamIds.Contains(teamId)))
            {
                return true;
            }

            return false;
        }

        public List<Guid> GetEligibleApprovers(
            ApprovalStep step,
            Dictionary<Guid, List<Guid>> userRoles,
            Dictionary<Guid, List<Guid>> userTeams
        )
        {
            var eligibleUsers = new HashSet<Guid>();

            // Add direct user approvers
            foreach (var userId in step.ApproverUserIds)
            {
                eligibleUsers.Add(userId);
            }

            // Add users with required roles
            foreach (var roleId in step.ApproverRoleIds)
            {
                var usersWithRole = userRoles
                    .Where(kvp => kvp.Value.Contains(roleId))
                    .Select(kvp => kvp.Key);
                foreach (var userId in usersWithRole)
                {
                    eligibleUsers.Add(userId);
                }
            }

            // Add users in required teams
            foreach (var teamId in step.ApproverTeamIds)
            {
                var usersInTeam = userTeams
                    .Where(kvp => kvp.Value.Contains(teamId))
                    .Select(kvp => kvp.Key);
                foreach (var userId in usersInTeam)
                {
                    eligibleUsers.Add(userId);
                }
            }

            return eligibleUsers.ToList();
        }
    }
}
