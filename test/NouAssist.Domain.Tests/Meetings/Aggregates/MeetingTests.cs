using System;
using NouAssist.Meetings.Enums;
using NouAssist.Meetings.ValueObjects;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Meetings.Aggregates
{
    public class MeetingTests
    {
        [Fact]
        public void Start_Should_UpdateStatus_WhenScheduled()
        {
            // Arrange
            var meeting = CreateMeeting();

            // Act
            meeting.Start();

            // Assert
            meeting.IsInProgress.ShouldBeTrue();
            meeting.ActualStartTime.ShouldNotBeNull();
        }

        [Fact]
        public void Start_Should_ThrowException_WhenNotScheduled()
        {
            // Arrange
            var meeting = CreateMeeting();
            meeting.Start();

            // Act & Assert
            Should.Throw<BusinessException>(() => meeting.Start());
        }

        [Fact]
        public void AddAttendee_Should_ThrowException_WhenDuplicate()
        {
            // Arrange
            var meeting = CreateMeeting();
            var userId = Guid.NewGuid();
            var attendee = new MeetingAttendee(
                userId, "test@test.com", "Test", 
                AttendeeRole.Participant, AttendeeStatus.Pending, false, false
            );
            meeting.AddAttendee(attendee);

            // Act & Assert
            Should.Throw<BusinessException>(() => meeting.AddAttendee(attendee));
        }

        [Fact]
        public void Complete_Should_SetCompletionData()
        {
            // Arrange
            var meeting = CreateMeeting();
            meeting.Start();

            // Act
            meeting.Complete("Test notes", "Test decisions", "Test summary");

            // Assert
            meeting.IsCompleted.ShouldBeTrue();
            meeting.Notes.ShouldBe("Test notes");
            meeting.ActualEndTime.ShouldNotBeNull();
        }

        [Fact]
        public void GetMetrics_Should_ReturnCompleteMetrics()
        {
            // Arrange
            var meeting = CreateMeeting();
            var attendee = new MeetingAttendee(
                Guid.NewGuid(), "test@test.com", "Test",
                AttendeeRole.Participant, AttendeeStatus.Accepted, false, false
            );
            meeting.AddAttendee(attendee);

            // Act
            var metrics = meeting.GetMetrics();

            // Assert
            metrics.ShouldNotBeNull();
            metrics.AttendeeCount.ShouldBe(1);
            metrics.ConfirmedCount.ShouldBe(1);
        }

        private Meeting CreateMeeting()
        {
            return new Meeting(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Meeting",
                DateTime.UtcNow.AddHours(1),
                DateTime.UtcNow.AddHours(2),
                MeetingType.General,
                null
            );
        }
    }
}
