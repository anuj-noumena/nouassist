using System;
using NouAssist.Calendar.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Calendar.Aggregates
{
    public class CalendarEventTests
    {
        [Fact]
        public void Constructor_Should_CreateValidEvent()
        {
            // Arrange
            var calendarId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var title = "Team Meeting";
            var startAt = DateTime.UtcNow.AddHours(1);
            var endAt = startAt.AddHours(1);

            // Act
            var calendarEvent = new CalendarEvent(
                Guid.NewGuid(),
                calendarId,
                ownerId,
                title,
                startAt,
                endAt);

            // Assert
            calendarEvent.ShouldNotBeNull();
            calendarEvent.CalendarId.ShouldBe(calendarId);
            calendarEvent.OwnerId.ShouldBe(ownerId);
            calendarEvent.Title.ShouldBe(title);
            calendarEvent.StartAt.ShouldBe(startAt);
            calendarEvent.EndAt.ShouldBe(endAt);
            calendarEvent.IsCancelled.ShouldBeFalse();
            calendarEvent.Status.ShouldBe(EventStatus.Upcoming);
        }

        [Fact]
        public void Constructor_Should_ThrowException_WhenTitleIsEmpty()
        {
            // Arrange
            var calendarId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var startAt = DateTime.UtcNow.AddHours(1);
            var endAt = startAt.AddHours(1);

            // Act & Assert
            Should.Throw<ArgumentException>(() => new CalendarEvent(
                Guid.NewGuid(),
                calendarId,
                ownerId,
                "", // Empty title
                startAt,
                endAt));
        }

        [Fact]
        public void Constructor_Should_ThrowException_WhenEndBeforeStart()
        {
            // Arrange
            var calendarId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var startAt = DateTime.UtcNow.AddHours(1);
            var endAt = startAt.AddHours(-1); // End before start

            // Act & Assert
            Should.Throw<BusinessException>(() => new CalendarEvent(
                Guid.NewGuid(),
                calendarId,
                ownerId,
                "Meeting",
                startAt,
                endAt));
        }

        [Fact]
        public void Cancel_Should_SetCancelledFlag()
        {
            // Arrange
            var calendarEvent = CreateValidEvent();
            var cancelledBy = Guid.NewGuid().ToString();
            var reason = "Meeting no longer needed";

            // Act
            calendarEvent.Cancel(cancelledBy, reason);

            // Assert
            calendarEvent.IsCancelled.ShouldBeTrue();
            calendarEvent.CancelledAt.ShouldNotBeNull();
            calendarEvent.CancelledBy.ShouldBe(cancelledBy);
            calendarEvent.CancellationReason.ShouldBe(reason);
            calendarEvent.Status.ShouldBe(EventStatus.Cancelled);
        }

        [Fact]
        public void Cancel_Should_ThrowException_WhenAlreadyCancelled()
        {
            // Arrange
            var calendarEvent = CreateValidEvent();
            calendarEvent.Cancel(Guid.NewGuid().ToString());

            // Act & Assert
            Should.Throw<BusinessException>(() => calendarEvent.Cancel(Guid.NewGuid().ToString()));
        }

        [Fact]
        public void Reschedule_Should_UpdateTimes()
        {
            // Arrange
            var calendarEvent = CreateValidEvent();
            var newStart = DateTime.UtcNow.AddDays(1);
            var newEnd = newStart.AddHours(2);

            // Act
            calendarEvent.Reschedule(newStart, newEnd);

            // Assert
            calendarEvent.StartAt.ShouldBe(newStart);
            calendarEvent.EndAt.ShouldBe(newEnd);
            calendarEvent.DurationInMinutes.ShouldBe(120);
        }

        [Fact]
        public void LinkToEntity_Should_AddLinkedEntity()
        {
            // Arrange
            var calendarEvent = CreateValidEvent();
            var entityId = Guid.NewGuid();
            var entityType = LinkedEntityType.Task;

            // Act
            calendarEvent.LinkToEntity(entityId, entityType);

            // Assert
            calendarEvent.LinkedEntities.ShouldContain(e => 
                e.EntityId == entityId && e.EntityType == entityType);
        }

        [Fact]
        public void LinkToEntity_Should_ThrowException_WhenDuplicate()
        {
            // Arrange
            var calendarEvent = CreateValidEvent();
            var entityId = Guid.NewGuid();
            var entityType = LinkedEntityType.Task;
            calendarEvent.LinkToEntity(entityId, entityType);

            // Act & Assert
            Should.Throw<BusinessException>(() => 
                calendarEvent.LinkToEntity(entityId, entityType));
        }

        [Fact]
        public void Duration_Should_CalculateCorrectly()
        {
            // Arrange
            var startAt = DateTime.UtcNow;
            var endAt = startAt.AddMinutes(90);
            var calendarEvent = new CalendarEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Meeting",
                startAt,
                endAt);

            // Assert
            calendarEvent.Duration.ShouldBe(TimeSpan.FromMinutes(90));
            calendarEvent.DurationInMinutes.ShouldBe(90);
        }

        private CalendarEvent CreateValidEvent()
        {
            var startAt = DateTime.UtcNow.AddHours(1);
            return new CalendarEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Event",
                startAt,
                startAt.AddHours(1));
        }
    }
}
