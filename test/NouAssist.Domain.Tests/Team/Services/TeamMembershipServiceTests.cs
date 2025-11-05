using System;
using NouAssist.Team.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.Team.Services
{
    public class TeamMembershipServiceTests
    {
        private readonly TeamMembershipService _service;

        public TeamMembershipServiceTests()
        {
            _service = new TeamMembershipService();
        }

        [Fact]
        public void AddMemberToTeam_Should_Succeed_WhenUserIsNotMember()
        {
            // Arrange
            var team = CreateTeam();
            var leaderId = Guid.NewGuid();
            team.AssignLeader(team.AddMember(leaderId, TeamMemberRole.Owner).UserId);
            
            var newUserId = Guid.NewGuid();

            // Act
            var result = _service.AddMemberToTeam(
                team,
                newUserId,
                TeamMemberRole.Member,
                leaderId
            );

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Member.ShouldNotBeNull();
            team.IsMember(newUserId).ShouldBeTrue();
        }

        [Fact]
        public void AddMemberToTeam_Should_Fail_WhenTeamInactive()
        {
            // Arrange
            var team = CreateTeam();
            team.Deactivate();
            var leaderId = Guid.NewGuid();
            var newUserId = Guid.NewGuid();

            // Act
            var result = _service.AddMemberToTeam(
                team,
                newUserId,
                TeamMemberRole.Member,
                leaderId
            );

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.HasErrors.ShouldBeTrue();
        }

        [Fact]
        public void RemoveMemberFromTeam_Should_Fail_WhenRemovingLastAdmin()
        {
            // Arrange
            var team = CreateTeam();
            var adminId = Guid.NewGuid();
            team.AddMember(adminId, TeamMemberRole.Admin);
            team.AddAdmin(adminId);
            team.AssignLeader(adminId);

            // Act
            var result = _service.RemoveMemberFromTeam(
                team,
                adminId,
                adminId,
                "Testing"
            );

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.Contains("last admin"));
        }

        [Fact]
        public void PromoteMember_Should_AddToAdminList_WhenPromotingToAdmin()
        {
            // Arrange
            var team = CreateTeam();
            var leaderId = Guid.NewGuid();
            team.AddMember(leaderId, TeamMemberRole.Owner);
            team.AssignLeader(leaderId);

            var memberId = Guid.NewGuid();
            team.AddMember(memberId, TeamMemberRole.Member);

            // Act
            var result = _service.PromoteMember(
                team,
                memberId,
                TeamMemberRole.Admin,
                leaderId
            );

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            team.IsAdmin(memberId).ShouldBeTrue();
        }

        private Aggregates.Team CreateTeam()
        {
            return new Aggregates.Team(
                Guid.NewGuid(),
                "Test Team",
                TeamType.Project,
                null
            );
        }
    }
}
