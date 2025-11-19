using System;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.GTD
{
    public class FollowupTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateFollowup()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var followupAt = DateTime.Now.AddDays(7);

            // Act
            var followup = new Followup(
                Guid.NewGuid(),
                ownerId,
                followupAt);

            // Assert
            followup.ShouldNotBeNull();
            followup.OwnerId.ShouldBe(ownerId);
            followup.FollowupAt.ShouldBe(followupAt);
            followup.Status.ShouldBe(FollowupStatus.Scheduled);
        }

        [Fact]
        public void MarkAsDone_Should_ChangeStatusToDone()
        {
            // Arrange
            var followup = new Followup(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.Now.AddDays(7));

            // Act
            followup.MarkAsDone();

            // Assert
            followup.Status.ShouldBe(FollowupStatus.Done);
            followup.CompletedAt.ShouldNotBeNull();
        }

        [Fact]
        public void MarkAsDone_WhenAlreadyDone_Should_RemainDone()
        {
            // Arrange
            var followup = new Followup(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.Now.AddDays(7));
            followup.MarkAsDone();
            var completedAt = followup.CompletedAt;

            // Act
            followup.MarkAsDone();

            // Assert
            followup.Status.ShouldBe(FollowupStatus.Done);
            followup.CompletedAt.ShouldBe(completedAt);
        }

        [Fact]
        public void Dismiss_Should_ChangeStatusToDismissed()
        {
            // Arrange
            var followup = new Followup(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.Now.AddDays(7));

            // Act
            followup.Dismiss();

            // Assert
            followup.Status.ShouldBe(FollowupStatus.Dismissed);
        }

        [Fact]
        public void Reschedule_Should_UpdateFollowupDate()
        {
            // Arrange
            var followup = new Followup(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.Now.AddDays(7));
            var newDate = DateTime.Now.AddDays(14);

            // Act
            followup.Reschedule(newDate);

            // Assert
            followup.FollowupAt.ShouldBe(newDate);
            followup.Status.ShouldBe(FollowupStatus.Scheduled);
        }
    }
}
