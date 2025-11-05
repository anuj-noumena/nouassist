using System;
using System.Threading.Tasks;
using NouAssist.Reminders.Aggregates;
using NouAssist.Reminders.Data;
using NouAssist.Reminders.Enums;
using NouAssist.Reminders.Reminders.Dtos;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace NouAssist.Reminders.Reminders
{
    public class ReminderAppService_Tests : RemindersApplicationTestBase
    {
        private readonly IReminderAppService _reminderAppService;
        private readonly IReminderRepository _reminderRepository;

        public ReminderAppService_Tests()
        {
            _reminderAppService = GetRequiredService<IReminderAppService>();
            _reminderRepository = GetRequiredService<IReminderRepository>();
        }

        [Fact]
        public async Task CreateAsync_Should_Create_A_Valid_Reminder()
        {
            // Arrange
            var input = new CreateReminderDto
            {
                Title = "Team Meeting",
                RemindAt = DateTime.UtcNow.AddHours(2),
                Priority = ReminderPriority.High,
                Notes = "Prepare presentation"
            };

            // Act
            var result = await _reminderAppService.CreateAsync(input);

            // Assert
            result.Id.ShouldNotBe(Guid.Empty);
            result.Title.ShouldBe(input.Title);
            result.Priority.ShouldBe(input.Priority.ToString());
            
            var reminderInDb = await _reminderRepository.FindAsync(result.Id);
            reminderInDb.ShouldNotBeNull();
            reminderInDb.IsPending.ShouldBeTrue();
        }

        [Fact]
        public async Task CreateRecurringAsync_Should_Create_Recurring_Reminder()
        {
            // Arrange
            var input = new CreateRecurringReminderDto
            {
                Title = "Daily Standup",
                BaseDateTime = DateTime.UtcNow.Date.AddHours(9),
                MinutesBeforeEvent = 5,
                RecurrencePattern = RecurrencePattern.Daily,
                RecurrenceInterval = 1
            };

            // Act
            var result = await _reminderAppService.CreateRecurringAsync(input);

            // Assert
            result.Id.ShouldNotBe(Guid.Empty);
            result.IsRecurring.ShouldBeTrue();
            result.Schedule.ShouldNotBeNull();
            result.Schedule.RecurrencePattern.ShouldBe(RecurrencePattern.Daily.ToString());
        }

        [Fact]
        public async Task SnoozeAsync_Should_Postpone_Reminder()
        {
            // Arrange
            var reminder = await CreateTestReminderAsync();
            var originalRemindAt = reminder.RemindAt;

            // Act
            var result = await _reminderAppService.SnoozeAsync(
                reminder.Id, 
                new SnoozeReminderDto { Minutes = 30 }
            );

            // Assert
            result.RemindAt.ShouldBeGreaterThan(originalRemindAt);
            result.SnoozeCount.ShouldBe(1);
            result.SnoozeInfo.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetUpcomingRemindersAsync_Should_Return_Future_Reminders()
        {
            // Arrange
            await CreateTestReminderAsync(DateTime.UtcNow.AddHours(1));
            await CreateTestReminderAsync(DateTime.UtcNow.AddHours(12));
            await CreateTestReminderAsync(DateTime.UtcNow.AddDays(2)); // Outside range

            // Act
            var results = await _reminderAppService.GetUpcomingRemindersAsync(
                new GetUpcomingRemindersDto { HoursAhead = 24 }
            );

            // Assert
            results.ShouldNotBeNull();
            results.Count.ShouldBe(2);
        }

        private async Task<Reminder> CreateTestReminderAsync(DateTime? remindAt = null)
        {
            return await WithUnitOfWorkAsync(async () =>
            {
                var reminder = new Reminder(
                    GuidGenerator.Create(),
                    CurrentUser.Id.Value,
                    "Test Reminder",
                    remindAt ?? DateTime.UtcNow.AddHours(2),
                    null
                );
                return await _reminderRepository.InsertAsync(reminder);
            });
        }
    }
}
