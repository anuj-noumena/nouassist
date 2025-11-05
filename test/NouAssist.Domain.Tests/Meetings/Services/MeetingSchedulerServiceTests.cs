using System;
using System.Collections.Generic;
using NouAssist.Meetings.Aggregates;
using NouAssist.Meetings.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.Meetings.Services
{
    public class MeetingSchedulerServiceTests
    {
        private readonly MeetingSchedulerService _service;

        public MeetingSchedulerServiceTests()
        {
            _service = new MeetingSchedulerService();
        }

        [Fact]
        public void ScheduleMeeting_Should_CreateMeeting_WhenNoConflicts()
        {
            // Arrange
            var startAt = DateTime.UtcNow.AddDays(1);
            var endAt = startAt.AddHours(1);
            var existingMeetings = new List<Meeting>();

            // Act
            var result = _service.ScheduleMeeting(
                "Test Meeting",
                startAt,
                endAt,
                Guid.NewGuid(),
                MeetingType.General,
                existingMeetings,
                null
            );

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Meeting.ShouldNotBeNull();
            result.HasConflicts.ShouldBeFalse();
        }

        [Fact]
        public void ScheduleMeeting_Should_DetectConflict_WhenOverlappingMeeting()
        {
            // Arrange
            var startAt = DateTime.UtcNow.AddDays(1);
            var endAt = startAt.AddHours(1);
            
            var existingMeeting = new Meeting(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Existing Meeting",
                startAt.AddMinutes(-30),
                startAt.AddMinutes(30),
                MeetingType.General,
                null
            );

            var existingMeetings = new List<Meeting> { existingMeeting };

            // Act
            var result = _service.ScheduleMeeting(
                "New Meeting",
                startAt,
                endAt,
                Guid.NewGuid(),
                MeetingType.General,
                existingMeetings,
                null
            );

            // Assert
            result.HasConflicts.ShouldBeTrue();
            result.Conflicts.Count.ShouldBe(1);
        }

        [Fact]
        public void DetectScheduleConflicts_Should_ReturnEmpty_WhenNoOverlap()
        {
            // Arrange
            var startAt = DateTime.UtcNow.AddDays(1);
            var endAt = startAt.AddHours(1);
            
            var existingMeeting = new Meeting(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Existing Meeting",
                startAt.AddHours(-2),
                startAt.AddHours(-1),
                MeetingType.General,
                null
            );

            var existingMeetings = new List<Meeting> { existingMeeting };

            // Act
            var conflicts = _service.DetectScheduleConflicts(startAt, endAt, existingMeetings);

            // Assert
            conflicts.ShouldBeEmpty();
        }

        [Fact]
        public void GenerateRecurringOccurrences_Should_CreateMultipleOccurrences()
        {
            // Arrange
            var startAt = DateTime.UtcNow.AddDays(1);
            var endAt = startAt.AddHours(1);
            
            var templateMeeting = _service.CreateRecurringMeeting(
                "Weekly Meeting",
                startAt,
                endAt,
                MeetingFrequency.Weekly,
                MeetingType.TeamSync,
                Guid.NewGuid(),
                null,
                4
            );

            // Act
            var occurrences = _service.GenerateRecurringOccurrences(templateMeeting, 4);

            // Assert
            occurrences.Count.ShouldBe(3); // Generates n-1 occurrences (template is the first)
            occurrences[0].StartAt.ShouldBe(startAt.AddDays(7));
            occurrences[1].StartAt.ShouldBe(startAt.AddDays(14));
        }
    }
}
