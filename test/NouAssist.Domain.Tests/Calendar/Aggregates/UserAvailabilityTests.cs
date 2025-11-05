using System;
using Shouldly;
using Xunit;

namespace NouAssist.Calendar.Aggregates
{
    public class UserAvailabilityTests
    {
        [Fact]
        public void SetWeeklyAvailability_Should_AddSlot()
        {
            // Arrange
            var availability = new UserAvailability(Guid.NewGuid(), Guid.NewGuid());

            // Act
            availability.SetWeeklyAvailability(1, "09:00", "17:00", true);

            // Assert
            availability.WeeklySlots.Count.ShouldBe(1);
        }

        [Fact]
        public void SetDateOverride_Should_OverrideWeeklyAvailability()
        {
            // Arrange
            var availability = new UserAvailability(Guid.NewGuid(), Guid.NewGuid());
            availability.SetWeeklyAvailability(1, "09:00", "17:00", true);
            var overrideDate = DateTime.Today.AddDays(1);

            // Act
            availability.SetDateOverride(overrideDate, false, "Holiday");

            // Assert
            availability.IsAvailableOn(overrideDate).ShouldBeFalse();
        }

        [Fact]
        public void IsAvailableOn_Should_ReturnTrue_WhenNoOverrideAndWeeklyAvailable()
        {
            // Arrange
            var availability = new UserAvailability(Guid.NewGuid(), Guid.NewGuid());
            var monday = GetNextMonday();
            availability.SetWeeklyAvailability((int)monday.DayOfWeek, "09:00", "17:00", true);

            // Act
            var isAvailable = availability.IsAvailableOn(monday);

            // Assert
            isAvailable.ShouldBeTrue();
        }

        private DateTime GetNextMonday()
        {
            var today = DateTime.Today;
            var daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            return today.AddDays(daysUntilMonday);
        }
    }
}
