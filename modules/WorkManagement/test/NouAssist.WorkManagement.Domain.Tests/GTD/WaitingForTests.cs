using System;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.GTD
{
    public class WaitingForTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateWaitingFor()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var wfType = WaitingForType.User;

            // Act
            var waitingFor = new WaitingFor(
                Guid.NewGuid(),
                ownerId,
                wfType);

            // Assert
            waitingFor.ShouldNotBeNull();
            waitingFor.OwnerId.ShouldBe(ownerId);
            waitingFor.WfType.ShouldBe(wfType);
            waitingFor.Status.ShouldBe(WaitingForStatus.Active);
        }

        [Fact]
        public void Resolve_Should_ChangeStatusToResolved()
        {
            // Arrange
            var waitingFor = new WaitingFor(
                Guid.NewGuid(),
                Guid.NewGuid(),
                WaitingForType.User);

            // Act
            waitingFor.Resolve();

            // Assert
            waitingFor.Status.ShouldBe(WaitingForStatus.Resolved);
            waitingFor.ClearedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Resolve_WhenAlreadyResolved_Should_RemainResolved()
        {
            // Arrange
            var waitingFor = new WaitingFor(
                Guid.NewGuid(),
                Guid.NewGuid(),
                WaitingForType.User);
            waitingFor.Resolve();
            var firstClearedAt = waitingFor.ClearedAt;

            // Act
            waitingFor.Resolve();

            // Assert
            waitingFor.Status.ShouldBe(WaitingForStatus.Resolved);
            waitingFor.ClearedAt.ShouldBe(firstClearedAt);
        }

        [Fact]
        public void PutOnHold_Should_ChangeStatusToOnHold()
        {
            // Arrange
            var waitingFor = new WaitingFor(
                Guid.NewGuid(),
                Guid.NewGuid(),
                WaitingForType.User);

            // Act
            waitingFor.PutOnHold();

            // Assert
            waitingFor.Status.ShouldBe(WaitingForStatus.OnHold);
        }

        [Fact]
        public void Reactivate_Should_ChangeStatusToActive()
        {
            // Arrange
            var waitingFor = new WaitingFor(
                Guid.NewGuid(),
                Guid.NewGuid(),
                WaitingForType.User);
            waitingFor.PutOnHold();

            // Act
            waitingFor.Reactivate();

            // Assert
            waitingFor.Status.ShouldBe(WaitingForStatus.Active);
        }

        [Fact]
        public void Cancel_Should_ChangeStatusToCancelled()
        {
            // Arrange
            var waitingFor = new WaitingFor(
                Guid.NewGuid(),
                Guid.NewGuid(),
                WaitingForType.User);

            // Act
            waitingFor.Cancel();

            // Assert
            waitingFor.Status.ShouldBe(WaitingForStatus.Cancelled);
            waitingFor.ClearedAt.ShouldNotBeNull();
        }

        [Fact]
        public void SetFollowUpDate_Should_UpdateFollowUpDate()
        {
            // Arrange
            var waitingFor = new WaitingFor(
                Guid.NewGuid(),
                Guid.NewGuid(),
                WaitingForType.User);
            var followUpDate = DateTime.Now.AddDays(7);

            // Act
            waitingFor.FollowUpDate = followUpDate;

            // Assert
            waitingFor.FollowUpDate.ShouldBe(followUpDate);
        }

        [Fact]
        public void SetReason_Should_UpdateReason()
        {
            // Arrange
            var waitingFor = new WaitingFor(
                Guid.NewGuid(),
                Guid.NewGuid(),
                WaitingForType.User);
            var reason = "Waiting for approval";

            // Act
            waitingFor.Reason = reason;

            // Assert
            waitingFor.Reason.ShouldBe(reason);
        }

        [Fact]
        public void SetNote_Should_UpdateNote()
        {
            // Arrange
            var waitingFor = new WaitingFor(
                Guid.NewGuid(),
                Guid.NewGuid(),
                WaitingForType.User);
            var note = "Follow up next week";

            // Act
            waitingFor.Note = note;

            // Assert
            waitingFor.Note.ShouldBe(note);
        }
    }
}
