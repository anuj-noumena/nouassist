using System;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Calendar.Aggregates
{
    public class TimeblockTemplateTests
    {
        [Fact]
        public void Constructor_Should_CreateValidTemplate()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var title = "Morning Focus Block";
            var durationMinutes = 90;

            // Act
            var template = new TimeblockTemplate(
                Guid.NewGuid(),
                ownerId,
                title,
                durationMinutes);

            // Assert
            template.ShouldNotBeNull();
            template.Title.ShouldBe(title);
            template.DurationMinutes.ShouldBe(durationMinutes);
            template.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void SetDuration_Should_ThrowException_WhenTooShort()
        {
            // Arrange
            var template = CreateValidTemplate();

            // Act & Assert
            Should.Throw<BusinessException>(() => template.SetDuration(10));
        }

        [Fact]
        public void AddDayOfWeek_Should_AddToCollection()
        {
            // Arrange
            var template = CreateValidTemplate();

            // Act
            template.AddDayOfWeek(1); // Monday
            template.AddDayOfWeek(3); // Wednesday
            template.AddDayOfWeek(5); // Friday

            // Assert
            template.DaysOfWeek.Count.ShouldBe(3);
            template.HasSchedule.ShouldBeTrue();
        }

        [Fact]
        public void AddDayOfWeek_Should_NotAddDuplicate()
        {
            // Arrange
            var template = CreateValidTemplate();
            template.AddDayOfWeek(1);

            // Act
            template.AddDayOfWeek(1); // Try to add again

            // Assert
            template.DaysOfWeek.Count.ShouldBe(1);
        }

        private TimeblockTemplate CreateValidTemplate()
        {
            return new TimeblockTemplate(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Template",
                60);
        }
    }
}
