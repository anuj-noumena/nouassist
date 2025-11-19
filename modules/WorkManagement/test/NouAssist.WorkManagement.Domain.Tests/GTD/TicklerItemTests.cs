using System;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.GTD
{
    public class TicklerItemTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateTicklerItem()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var title = "Review insurance policy";
            var tickleDate = DateTime.Now.AddMonths(6);

            // Act
            var item = new TicklerItem(
                Guid.NewGuid(),
                ownerId,
                title,
                tickleDate);

            // Assert
            item.ShouldNotBeNull();
            item.OwnerId.ShouldBe(ownerId);
            item.Title.ShouldBe(title);
            item.TickleDate.ShouldBe(tickleDate);
            item.Status.ShouldBe(TicklerStatus.Scheduled);
        }

        [Fact]
        public void Surface_Should_ChangeStatusToSurfaced()
        {
            // Arrange
            var item = new TicklerItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Review insurance policy",
                DateTime.Now);

            // Act
            item.Surface();

            // Assert
            item.Status.ShouldBe(TicklerStatus.Surfaced);
            item.SurfacedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Action_Should_ChangeStatusToActioned()
        {
            // Arrange
            var item = new TicklerItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Review insurance policy",
                DateTime.Now);
            item.Surface();

            // Act
            item.Action(TicklerActionType.CreatedTask);

            // Assert
            item.Status.ShouldBe(TicklerStatus.Actioned);
            item.ActionType.ShouldBe(TicklerActionType.CreatedTask);
            item.ActionedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Dismiss_Should_ChangeStatusToDismissed()
        {
            // Arrange
            var item = new TicklerItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Review insurance policy",
                DateTime.Now);
            item.Surface();

            // Act
            item.Dismiss();

            // Assert
            item.Status.ShouldBe(TicklerStatus.Dismissed);
            item.DismissedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Snooze_Should_UpdateSnoozeUntil()
        {
            // Arrange
            var item = new TicklerItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Review insurance policy",
                DateTime.Now);
            item.Surface();
            var snoozeUntil = DateTime.Now.AddDays(3);

            // Act
            item.Snooze(snoozeUntil);

            // Assert
            item.Status.ShouldBe(TicklerStatus.Scheduled);
            item.SnoozeUntil.ShouldBe(snoozeUntil);
        }

        [Fact]
        public void Reschedule_Should_UpdateTickleDate()
        {
            // Arrange
            var item = new TicklerItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Review insurance policy",
                DateTime.Now);
            var newDate = DateTime.Now.AddMonths(1);

            // Act
            item.Reschedule(newDate);

            // Assert
            item.TickleDate.ShouldBe(newDate);
            item.Status.ShouldBe(TicklerStatus.Scheduled);
        }
    }
}
