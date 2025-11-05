using System;
using NouAssist.Calendar.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Calendar.Aggregates
{
    public class TimeblockTests
    {
        [Fact]
        public void Start_Should_ChangeStatusToInProgress()
        {
            // Arrange
            var timeblock = CreateValidTimeblock();
            timeblock.Status.ShouldBe(TimeblockStatus.Planned);

            // Act
            timeblock.Start();

            // Assert
            timeblock.Status.ShouldBe(TimeblockStatus.InProgress);
        }

        [Fact]
        public void Start_Should_ThrowException_WhenNotPlanned()
        {
            // Arrange
            var timeblock = CreateValidTimeblock();
            timeblock.Start(); // Now in progress

            // Act & Assert
            Should.Throw<BusinessException>(() => timeblock.Start());
        }

        [Fact]
        public void Complete_Should_ChangeStatusToCompleted()
        {
            // Arrange
            var timeblock = CreateValidTimeblock();
            var notes = "Completed successfully";

            // Act
            timeblock.Complete(notes);

            // Assert
            timeblock.Status.ShouldBe(TimeblockStatus.Completed);
            timeblock.CompletionNotes.ShouldBe(notes);
        }

        [Fact]
        public void Complete_Should_ThrowException_WhenAlreadyCompleted()
        {
            // Arrange
            var timeblock = CreateValidTimeblock();
            timeblock.Complete("First completion");

            // Act & Assert
            Should.Throw<BusinessException>(() => timeblock.Complete("Second completion"));
        }

        [Fact]
        public void Cancel_Should_ChangeStatusToCancelled()
        {
            // Arrange
            var timeblock = CreateValidTimeblock();

            // Act
            timeblock.Cancel();

            // Assert
            timeblock.Status.ShouldBe(TimeblockStatus.Cancelled);
        }

        [Fact]
        public void AddTask_Should_AddTaskToCollection()
        {
            // Arrange
            var timeblock = CreateValidTimeblock();
            var taskId = Guid.NewGuid();
            var estimatedMinutes = 30;

            // Act
            timeblock.AddTask(taskId, estimatedMinutes);

            // Assert
            timeblock.Tasks.Count.ShouldBe(1);
            timeblock.TotalEstimatedMinutes.ShouldBe(estimatedMinutes);
        }

        [Fact]
        public void Constructor_Should_ThrowException_WhenDurationTooShort()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var calendarEventId = Guid.NewGuid();
            var startAt = DateTime.UtcNow;
            var endAt = startAt.AddMinutes(10); // Less than 15 minutes

            // Act & Assert
            Should.Throw<BusinessException>(() => new Timeblock(
                Guid.NewGuid(),
                ownerId,
                calendarEventId,
                startAt,
                endAt));
        }

        [Fact]
        public void Constructor_Should_ThrowException_WhenDurationTooLong()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var calendarEventId = Guid.NewGuid();
            var startAt = DateTime.UtcNow;
            var endAt = startAt.AddHours(10); // More than 8 hours

            // Act & Assert
            Should.Throw<BusinessException>(() => new Timeblock(
                Guid.NewGuid(),
                ownerId,
                calendarEventId,
                startAt,
                endAt));
        }

        private Timeblock CreateValidTimeblock()
        {
            var startAt = DateTime.UtcNow.AddHours(1);
            return new Timeblock(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                startAt,
                startAt.AddMinutes(60));
        }
    }
}
