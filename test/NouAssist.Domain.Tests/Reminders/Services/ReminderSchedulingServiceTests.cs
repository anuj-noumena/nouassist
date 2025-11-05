using System;
using System.Collections.Generic;
using NouAssist.Reminders.Aggregates;
using NouAssist.Reminders.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.Reminders.Services
{
    public class ReminderSchedulingServiceTests
    {
        private readonly ReminderSchedulingService _service;

        public ReminderSchedulingServiceTests()
        {
            _service = new ReminderSchedulingService(null); // Repository not needed for these tests
        }

        [Fact]
        public void CreateSimpleReminder_Should_CreateValidReminder()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var title = "Test Reminder";
            var remindAt = DateTime.Now.AddHours(1);

            // Act
            var reminder = _service.CreateSimpleReminder(
                ownerId,
                title,
                remindAt,
                null
            );

            // Assert
            reminder.ShouldNotBeNull();
            reminder.OwnerId.ShouldBe(ownerId);
            reminder.Title.ShouldBe(title);
            reminder.RemindAt.ShouldBe(remindAt);
            reminder.Status.ShouldBe(ReminderStatus.Pending);
            reminder.IsSent.ShouldBeFalse();
        }

        [Fact]
        public void CreateRecurringReminder_Should_SetSchedule()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var title = "Daily Standup";
            var baseDateTime = DateTime.Now.Date.AddHours(9); // 9 AM
            var daysOfWeek = new List<DayOfWeek> 
            { 
                DayOfWeek.Monday, 
                DayOfWeek.Tuesday, 
                DayOfWeek.Wednesday, 
                DayOfWeek.Thursday, 
                DayOfWeek.Friday 
            };

            // Act
            var reminder = _service.CreateRecurringReminder(
                ownerId,
                title,
                baseDateTime,
                15, // 15 minutes before
                RecurrencePattern.Weekly,
                null,
                recurrenceDaysOfWeek: daysOfWeek
            );

            // Assert
            reminder.ShouldNotBeNull();
            reminder.IsRecurring.ShouldBeTrue();
            reminder.Schedule.ShouldNotBeNull();
            reminder.Schedule.RecurrencePattern.ShouldBe(RecurrencePattern.Weekly);
        }

        [Fact]
        public void SnoozeReminder_Should_UpdateRemindAtTime()
        {
            // Arrange
            var reminder = CreateTestReminder();
            var originalRemindAt = reminder.RemindAt;
            var snoozeDuration = 15;

            // Act
            _service.SnoozeReminder(reminder, snoozeDuration);

            // Assert
            reminder.RemindAt.ShouldBeGreaterThan(originalRemindAt);
            reminder.Status.ShouldBe(ReminderStatus.Snoozed);
            reminder.SnoozeCount.ShouldBe(1);
        }

        private Reminder CreateTestReminder()
        {
            return new Reminder(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Reminder",
                DateTime.Now.AddHours(1),
                null
            );
        }
    }
}
