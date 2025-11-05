using System;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using NouAssist.Tasks.ValueObjects;
using Shouldly;
using Xunit;

namespace NouAssist.Tasks.Services
{
    public class TaskRecurrenceServiceTests
    {
        private readonly TaskRecurrenceService _service;

        public TaskRecurrenceServiceTests()
        {
            _service = new TaskRecurrenceService(NullLogger<TaskRecurrenceService>.Instance);
        }

        [Fact]
        public void CalculateNextOccurrences_Should_GenerateDailyOccurrences()
        {
            // Arrange
            var recurrenceRule = new RecurrenceRule(
                RecurrenceFrequency.Daily,
                interval: 1,
                count: 5);
            var startDate = new DateTime(2025, 1, 1);

            // Act
            var occurrences = _service.CalculateNextOccurrences(recurrenceRule, startDate, 5);

            // Assert
            occurrences.Count.ShouldBe(5);
            occurrences[0].ShouldBe(new DateTime(2025, 1, 2));
            occurrences[1].ShouldBe(new DateTime(2025, 1, 3));
            occurrences[4].ShouldBe(new DateTime(2025, 1, 6));
        }

        [Fact]
        public void CalculateNextOccurrences_Should_RespectEndDate()
        {
            // Arrange
            var endDate = new DateTime(2025, 1, 5);
            var recurrenceRule = new RecurrenceRule(
                RecurrenceFrequency.Daily,
                interval: 1,
                endDate: endDate);
            var startDate = new DateTime(2025, 1, 1);

            // Act
            var occurrences = _service.CalculateNextOccurrences(recurrenceRule, startDate, 10);

            // Assert
            occurrences.Count.ShouldBeLessThanOrEqualTo(4);
            occurrences.All(o => o <= endDate).ShouldBeTrue();
        }

        [Fact]
        public void CalculateNextOccurrences_Should_GenerateWeeklyOccurrences()
        {
            // Arrange
            var recurrenceRule = new RecurrenceRule(
                RecurrenceFrequency.Weekly,
                interval: 2,
                count: 3);
            var startDate = new DateTime(2025, 1, 1); // Wednesday

            // Act
            var occurrences = _service.CalculateNextOccurrences(recurrenceRule, startDate, 3);

            // Assert
            occurrences.Count.ShouldBe(3);
            // Each occurrence should be 2 weeks apart
            (occurrences[1] - occurrences[0]).Days.ShouldBe(14);
        }
    }
}
