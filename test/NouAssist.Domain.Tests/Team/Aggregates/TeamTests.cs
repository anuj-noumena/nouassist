using System;
using NouAssist.Team.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Team.Aggregates
{
    public class TeamTests
    {
        [Fact]
        public void AddMember_Should_AddMember_WhenValid()
        {
            // Arrange
            var team = CreateTeam();
            var userId = Guid.NewGuid();

            // Act
            var member = team.AddMember(userId, TeamMemberRole.Member);

            // Assert
            member.ShouldNotBeNull();
            team.IsMember(userId).ShouldBeTrue();
        }

        [Fact]
        public void AddMember_Should_ThrowException_WhenMaxMembersReached()
        {
            // Arrange
            var team = CreateTeam();
            team.SetMaxMembers(1);
            team.AddMember(Guid.NewGuid(), TeamMemberRole.Member);

            // Act & Assert
            Should.Throw<BusinessException>(() =>
                team.AddMember(Guid.NewGuid(), TeamMemberRole.Member)
            );
        }

        [Fact]
        public void AssignLeader_Should_ThrowException_WhenUserNotMember()
        {
            // Arrange
            var team = CreateTeam();
            var userId = Guid.NewGuid();

            // Act & Assert
            Should.Throw<BusinessException>(() => team.AssignLeader(userId));
        }

        [Fact]
        public void GetActiveMemberCount_Should_ReturnCorrectCount()
        {
            // Arrange
            var team = CreateTeam();
            team.AddMember(Guid.NewGuid(), TeamMemberRole.Member);
            team.AddMember(Guid.NewGuid(), TeamMemberRole.Member);
            
            var removeMemberId = Guid.NewGuid();
            team.AddMember(removeMemberId, TeamMemberRole.Member);
            team.RemoveMember(removeMemberId, "Test");

            // Act
            var count = team.GetActiveMemberCount();

            // Assert
            count.ShouldBe(2);
        }

        private Team CreateTeam()
        {
            return new Team(
                Guid.NewGuid(),
                "Test Team",
                TeamType.Project,
                null
            );
        }
    }
}
