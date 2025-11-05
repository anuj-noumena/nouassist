using System;
using System.Collections.Generic;
using NouAssist.Calendar.Aggregates;
using Shouldly;
using Xunit;

namespace NouAssist.Calendar.Services
{
    public class CalendarEventSchedulingServiceTests
    {
        private readonly CalendarEventSchedulingService _service;

        public CalendarEventSchedulingServiceTests()
        {
            _service = new CalendarEventSchedulingService();
        }

        [Fact]
        public void ValidateEventScheduling_Should_ReturnErrors_WhenEventsOverlap()
        {
            // Arrange
            var calendarId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var startTime = DateTime.UtcNow.AddHours(1);

            var existingEvent = new CalendarEvent(
                Guid.NewGuid(),
                calendarId,
                ownerId,
                "Existing Meeting",
                startTime,
                startTime.AddHours(1));

            var newEvent = new CalendarEvent(
                Guid.NewGuid(),
                calendarId,
                ownerId,
                "New Meeting",
                startTime.AddMinutes(30),
                startTime.AddHours(1).AddMinutes(30));

            // Act
            var result = _service.ValidateEventScheduling(
                newEvent,
                new List<CalendarEvent> { existingEvent },
                new List<Timeblock>());

            // Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldNotBeEmpty();
            result.OverlappingEventIds.ShouldContain(existingEvent.Id);
        }

        [Fact]
        public void FindAvailableSlots_Should_ReturnFreeSlots()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(1);
            var endDate = startDate.AddDays(1);
            var durationMinutes = 60;

            var busyEvent = new CalendarEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Busy",
                startDate.AddHours(10),
                startDate.AddHours(11));

            // Act
            var availableSlots = _service.FindAvailableSlots(
                startDate,
                endDate,
                durationMinutes,
                new List<CalendarEvent> { busyEvent },
                new List<Timeblock>());

            // Assert
            availableSlots.ShouldNotBeEmpty();
            availableSlots.ShouldAllBe(slot => slot.DurationMinutes >= durationMinutes);
        }
    }
}
