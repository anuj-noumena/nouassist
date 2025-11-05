using System;
using System.Collections.Generic;
using NouAssist.Habits.Entities;
using Shouldly;
using Xunit;

namespace NouAssist.Habits.Services
{
    public class HabitAnalyticsServiceTests
    {
        private readonly HabitAnalyticsService _service;

        public HabitAnalyticsServiceTests()
        {
            _service = new HabitAnalyticsService();
        }

        [Fact]
        public void CalculateCurrentStreak_Should_Return_Zero_When_NoEntries()
        {
            // Arrange
            var entries = new List<HabitTrackingEntry>();

            // Act
            var streak = _service.CalculateCurrentStreak(entries);

            // Assert
            streak.ShouldBe(0);
        }

        [Fact]
        public void CalculateCurrentStreak_Should_Count_Consecutive_Days()
        {
            // Arrange
            var habitId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var today = DateTime.Now.Date;

            var entries = new List<HabitTrackingEntry>
            {
                CreateCompletedEntry(habitId, ownerId, today),
                CreateCompletedEntry(habitId, ownerId, today.AddDays(-1)),
                CreateCompletedEntry(habitId, ownerId, today.AddDays(-2))
            };

            // Act
            var streak = _service.CalculateCurrentStreak(entries);

            // Assert
            streak.ShouldBe(3);
        }

        [Fact]
        public void CalculateCompletionRate_Should_Return_Correct_Percentage()
        {
            // Arrange
            var habitId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var today = DateTime.Now.Date;

            var entries = new List<HabitTrackingEntry>
            {
                CreateCompletedEntry(habitId, ownerId, today),
                CreateCompletedEntry(habitId, ownerId, today.AddDays(-1)),
                CreateEntry(habitId, ownerId, today.AddDays(-2), completed: false),
                CreateCompletedEntry(habitId, ownerId, today.AddDays(-3))
            };

            // Act
            var rate = _service.CalculateCompletionRate(entries);

            // Assert
            rate.ShouldBe(0.75); // 3 out of 4
        }

        [Fact]
        public void CalculateConsistencyScore_Should_Be_Between_0_And_100()
        {
            // Arrange
            var habitId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var today = DateTime.Now.Date;

            var entries = new List<HabitTrackingEntry>
            {
                CreateCompletedEntry(habitId, ownerId, today),
                CreateCompletedEntry(habitId, ownerId, today.AddDays(-1)),
                CreateEntry(habitId, ownerId, today.AddDays(-2), completed: false)
            };

            // Act
            var score = _service.CalculateConsistencyScore(entries);

            // Assert
            score.ShouldBeGreaterThanOrEqualTo(0);
            score.ShouldBeLessThanOrEqualTo(100);
        }

        [Fact]
        public void GetBestDayOfWeek_Should_Return_Most_Completed_Day()
        {
            // Arrange
            var habitId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var monday = new DateTime(2024, 1, 1); // A Monday

            var entries = new List<HabitTrackingEntry>
            {
                CreateCompletedEntry(habitId, ownerId, monday),
                CreateCompletedEntry(habitId, ownerId, monday.AddDays(7)),
                CreateCompletedEntry(habitId, ownerId, monday.AddDays(14)),
                CreateEntry(habitId, ownerId, monday.AddDays(1), completed: false),
                CreateEntry(habitId, ownerId, monday.AddDays(2), completed: false)
            };

            // Act
            var bestDay = _service.GetBestDayOfWeek(entries);

            // Assert
            bestDay.ShouldNotBeNull();
            bestDay.Value.ShouldBe((int)DayOfWeek.Monday);
        }

        #region Test Helpers

        private HabitTrackingEntry CreateCompletedEntry(Guid habitId, Guid ownerId, DateTime date)
        {
            return CreateEntry(habitId, ownerId, date, completed: true);
        }

        private HabitTrackingEntry CreateEntry(Guid habitId, Guid ownerId, DateTime date, bool completed)
        {
            var entry = new HabitTrackingEntry(
                Guid.NewGuid(),
                ownerId,
                habitId,
                date,
                completed
            );

            return entry;
        }

        #endregion
    }
}
